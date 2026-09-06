using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ECommerceOrderManagement.Infrastructure.Messaging.RabbitMq;

internal sealed class RabbitMqConnection : IAsyncDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private IConnection? _connection;

    public RabbitMqConnection(IOptions<RabbitMqOptions> options)
    {
        var rabbitMqOptions = options.Value;

        _connectionFactory = new ConnectionFactory
        {
            HostName = rabbitMqOptions.HostName,
            Port = rabbitMqOptions.Port,
            UserName = rabbitMqOptions.UserName,
            Password = rabbitMqOptions.Password,
            AutomaticRecoveryEnabled = true,
            ClientProvidedName = "ecommerce-order-management-api"
        };
    }

    public async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (_connection is not null)
        {
            return _connection;
        }

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            if (_connection is not null)
            {
                return _connection;
            }


            _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            return _connection;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }

        _semaphore.Dispose();
    }
}