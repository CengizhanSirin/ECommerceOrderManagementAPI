using ECommerceOrderManagement.Application.Common.Abstractions.Email;
using ECommerceOrderManagement.Application.Common.Abstractions.Identity;
using ECommerceOrderManagement.Application.Common.Abstractions.Outbox;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Features.Orders;
using ECommerceOrderManagement.Domain.Payments.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ECommerceOrderManagement.Infrastructure.BackgroundServices.Outbox;

internal sealed class OutboxBackgroundService(
    IServiceScopeFactory scopeFactory,
    IOptions<OutboxProcessingOptions> options,
    ILogger<OutboxBackgroundService> logger,
    TimeProvider timeProvider)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMessagesAsync(stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {

                break;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred while processing outbox messages.");
            }



            try
            {
                await Task.Delay(TimeSpan.FromSeconds(options.Value.PollingIntervalSeconds), stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }
    }

    private async Task ProcessPendingMessagesAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        var outboxQueries = scope.ServiceProvider
            .GetRequiredService<IOutboxQueries>();

        var outboxRepository = scope.ServiceProvider
            .GetRequiredService<IOutboxRepository>();

        var orderQueries = scope.ServiceProvider
            .GetRequiredService<IOrderQueries>();

        var identityService = scope.ServiceProvider
            .GetRequiredService<IIdentityService>();

        var emailSender = scope.ServiceProvider
            .GetRequiredService<IEmailSender>();

        var unitOfWork = scope.ServiceProvider
            .GetRequiredService<IUnitOfWork>();

        var messages = await outboxQueries.GetPendingAsync(
            options.Value.BatchSize,
            options.Value.MaxRetryCount,
            cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                if (message.Type == nameof(PaymentSucceededDomainEvent))
                {
                    await ProcessPaymentSucceededAsync(
                        message.Payload,
                        orderQueries,
                        identityService,
                        emailSender,
                        cancellationToken);
                }
                else
                {
                    throw new InvalidOperationException( $"Unsupported outbox message type: {message.Type}");
                }

                await outboxRepository.MarkAsProcessedAsync( message.Id, timeProvider.GetUtcNow().UtcDateTime,cancellationToken);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                logger.LogInformation( "Outbox message {MessageId} processed successfully.", message.Id);
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {

                logger.LogError( exception,  "Failed to process outbox message {MessageId}.",   message.Id);


                await outboxRepository.MarkAsFailedAsync(
                    message.Id,
                    exception.Message,
                    cancellationToken);

                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }


    private static async Task ProcessPaymentSucceededAsync(
    string payload,
    IOrderQueries orderQueries,
    IIdentityService identityService,
    IEmailSender emailSender,
    CancellationToken cancellationToken)
    {
        var domainEvent =JsonSerializer.Deserialize<PaymentSucceededDomainEvent>(payload);

        if (domainEvent is null)
        {
            throw new InvalidOperationException( "Payment succeeded event payload could not be deserialized.");
        }

        var order = await orderQueries.GetByIdAsync(domainEvent.OrderId,cancellationToken);

        if (order is null)
        {
            throw new InvalidOperationException( $"Order {domainEvent.OrderId} was not found.");
        }

        var customer = await identityService.GetUserByIdAsync( order.CustomerId,  cancellationToken);

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