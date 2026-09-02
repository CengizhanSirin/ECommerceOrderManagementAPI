
namespace ECommerceOrderManagement.Persistence.Outbox;

public sealed class OutboxMessage
{
    private OutboxMessage()
    {
    }

    public OutboxMessage(string type, string payload, DateTime occurredOnUtc)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Outbox message type is required.", nameof(type));

        if (string.IsNullOrWhiteSpace(payload))
            throw new ArgumentException("Outbox message payload is required.", nameof(payload));

        Id = Guid.NewGuid();
        Type = type;
        Payload = payload;
        OccurredOnUtc = occurredOnUtc;
        RetryCount = 0;
    }

    public Guid Id { get; private set; }

    public string Type { get; private set; } = string.Empty;

    public string Payload { get; private set; } = string.Empty;

    public DateTime OccurredOnUtc { get; private set; }

    public DateTime? ProcessedOnUtc { get; private set; }

    public int RetryCount { get; private set; }

    public string? LastError { get; private set; }

    public void MarkAsProcessed(DateTime processedOnUtc)
    {
        ProcessedOnUtc = processedOnUtc;
        LastError = null;
    }

    public void MarkAsFailed(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("Error message is required.", nameof(error));

        RetryCount++;
        LastError = error.Trim();
    }
}