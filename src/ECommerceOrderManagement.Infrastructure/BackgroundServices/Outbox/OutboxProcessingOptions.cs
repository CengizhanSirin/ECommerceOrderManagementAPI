namespace ECommerceOrderManagement.Infrastructure.BackgroundServices.Outbox;

public sealed class OutboxProcessingOptions
{
    public const string SectionName = "OutboxProcessing";

    public int PollingIntervalSeconds { get; init; } = 10;

    public int BatchSize { get; init; } = 10;

    public int MaxRetryCount { get; init; } = 3;
}