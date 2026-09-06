namespace ECommerceOrderManagement.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    public string HostName { get; init; } = string.Empty;
    public int Port { get; init; }

    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string ExchangeName { get; init; } = string.Empty;
    public string QueueName { get; init; } = string.Empty;
    public string RoutingKey { get; init; } = string.Empty;

    public string DeadLetterExchangeName { get; init; } = string.Empty;
    public string DeadLetterQueueName { get; init; } = string.Empty;
    public string DeadLetterRoutingKey { get; init; } = string.Empty;

    public int ConsumerMaxRetryCount { get; init; } = 3;
    public int ConsumerRetryDelaySeconds { get; init; } = 5;
}