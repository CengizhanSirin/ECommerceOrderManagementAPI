namespace ECommerceOrderManagement.Application.Common.Abstractions.Outbox;

public interface IOutboxRepository
{
    Task<bool> MarkAsProcessedAsync(Guid messageId, DateTime processedOnUtc, CancellationToken cancellationToken = default);

    Task<bool> MarkAsFailedAsync(Guid messageId, string error, CancellationToken cancellationToken = default);
}