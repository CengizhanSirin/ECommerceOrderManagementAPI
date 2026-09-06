using ECommerceOrderManagement.Application.Common.Abstractions.Email;
using ECommerceOrderManagement.Application.Common.Abstractions.Identity;
using ECommerceOrderManagement.Application.Features.Orders;
using ECommerceOrderManagement.Domain.Payments.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ECommerceOrderManagement.Infrastructure.Messaging.RabbitMq.Consumers;

internal sealed class PaymentSucceededConsumer : BackgroundService
{
    private readonly RabbitMqConnection _rabbitMqConnection;
    private readonly RabbitMqOptions _options;
    private readonly ILogger<PaymentSucceededConsumer> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public PaymentSucceededConsumer(RabbitMqConnection rabbitMqConnection, IOptions<RabbitMqOptions> options, ILogger<PaymentSucceededConsumer> logger, IServiceScopeFactory scopeFactory)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _options = options.Value;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = await _rabbitMqConnection.GetConnectionAsync(stoppingToken);

        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(
            exchange: _options.DeadLetterExchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: _options.DeadLetterQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync(
            queue: _options.DeadLetterQueueName,
            exchange: _options.DeadLetterExchangeName,
            routingKey: _options.DeadLetterRoutingKey,
            arguments: null,
            cancellationToken: stoppingToken);

        var queueArguments = new Dictionary<string, object?>
        {
            ["x-dead-letter-exchange"] = _options.DeadLetterExchangeName,

            ["x-dead-letter-routing-key"] = _options.DeadLetterRoutingKey
        };

        await channel.QueueDeclareAsync(
            queue: _options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArguments,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync(
            queue: _options.QueueName,
            exchange: _options.ExchangeName,
            routingKey: _options.RoutingKey,
            arguments: null,
            cancellationToken: stoppingToken);

        await channel.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();

            var payload = Encoding.UTF8.GetString(body);

            for (var attempt = 1; attempt <= _options.ConsumerMaxRetryCount; attempt++)
            {
                try
                {
                    await ProcessMessageAsync(payload, stoppingToken);

                    await channel.BasicAckAsync(
                        deliveryTag: eventArgs.DeliveryTag,
                        multiple: false,
                        cancellationToken: stoppingToken);

                    _logger.LogInformation("RabbitMQ message {DeliveryTag} processed successfully on attempt {Attempt}.",
                        eventArgs.DeliveryTag,
                        attempt);

                    return;
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    _logger.LogWarning(
                        exception,
                        "RabbitMQ message {DeliveryTag} failed on attempt {Attempt}/{MaxRetryCount}.",
                        eventArgs.DeliveryTag,
                        attempt,
                        _options.ConsumerMaxRetryCount);

                    if (attempt < _options.ConsumerMaxRetryCount)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(_options.ConsumerRetryDelaySeconds), stoppingToken);
                    }
                }
            }

            await channel.BasicNackAsync(
                deliveryTag: eventArgs.DeliveryTag,
                multiple: false,
                requeue: false,
                cancellationToken: stoppingToken);

            _logger.LogError(
                "RabbitMQ message {DeliveryTag} moved to dead-letter queue after {MaxRetryCount} failed attempts.",
                eventArgs.DeliveryTag,
                _options.ConsumerMaxRetryCount);
        };

        await channel.BasicConsumeAsync(
            queue: _options.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("RabbitMQ consumer started. Queue: {QueueName}", _options.QueueName);

        try
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
        }
        catch (OperationCanceledException)
            when (stoppingToken.IsCancellationRequested)
        {
        }
    }





    private async Task ProcessMessageAsync(string payload, CancellationToken cancellationToken)
    {
        var domainEvent = JsonSerializer.Deserialize<PaymentSucceededDomainEvent>(payload);

        if (domainEvent is null)
        {
            throw new InvalidOperationException("Payment succeeded event payload could not be deserialized.");
        }

        await using var scope = _scopeFactory.CreateAsyncScope();

        var orderQueries = scope.ServiceProvider.GetRequiredService<IOrderQueries>();

        var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();

        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

        var order = await orderQueries.GetByIdAsync(domainEvent.OrderId, cancellationToken);

        if (order is null)
        {
            throw new InvalidOperationException($"Order {domainEvent.OrderId} was not found.");
        }

        var customer = await identityService.GetUserByIdAsync(order.CustomerId, cancellationToken);

        if (customer is null)
        {
            throw new InvalidOperationException($"Customer {order.CustomerId} was not found.");
        }

        var subject = $"Siparişiniz onaylandı - {order.OrderNumber}";

        var htmlBody = $"""
                      <h2>Siparişiniz onaylandı</h2>

                     <p>Ödemeniz başarıyla alınmıştır.</p>

                     <p>
                     <strong>Sipariş No:</strong> 
                     {order.OrderNumber}
                     </p>
                   
                     <p>
                     <strong>Toplam Tutar:</strong> 
                     {order.TotalAmount:N2} TL
                     </p>
                   
                     <p>
                     Siparişiniz için teşekkür ederiz.
                     </p>
                     """;

        await emailSender.SendAsync(
            customer.Email,
            subject,
            htmlBody,
            cancellationToken);
    }
}