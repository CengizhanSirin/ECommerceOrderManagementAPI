using ECommerceOrderManagement.Application.Common.Abstractions.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace ECommerceOrderManagement.Infrastructure.Messaging.RabbitMq;

internal sealed class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly RabbitMqConnection _rabbitMqConnection;
    private readonly RabbitMqOptions _options;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private IChannel? _channel;

    public RabbitMqPublisher(RabbitMqConnection rabbitMqConnection, IOptions<RabbitMqOptions> options)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _options = options.Value;
    }

    public async Task PublishAsync(string payload, CancellationToken cancellationToken = default)
    {
        var channel = await GetChannelAsync(cancellationToken);

        var body = Encoding.UTF8.GetBytes(payload);

        var properties = new BasicProperties
        {
            ContentType = "application/json",
            Persistent = true
        };

        await channel.BasicPublishAsync(
            exchange: _options.ExchangeName,
            routingKey: _options.RoutingKey,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }

    private async Task<IChannel> GetChannelAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null && _channel.IsOpen)
        {
            return _channel;
        }

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            if (_channel is not null && _channel.IsOpen)
            {
                return _channel;
            }

            if (_channel is not null)
            {
                await _channel.DisposeAsync();
            }

            var connection = await _rabbitMqConnection.GetConnectionAsync(cancellationToken);

            var channelOptions = new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true);

            _channel = await connection.CreateChannelAsync(channelOptions, cancellationToken);

            await _channel.ExchangeDeclareAsync(
                exchange: _options.ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            return _channel;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.DisposeAsync();
        }

        _semaphore.Dispose();
    }
}