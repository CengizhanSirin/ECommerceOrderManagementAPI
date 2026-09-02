namespace ECommerceOrderManagement.Application.Common.Abstractions.Outbox;

public interface IOutboxQueries
{
    Task<IReadOnlyList<OutboxMessageReadModel>> GetPendingAsync(int batchSize, int maxRetryCount, CancellationToken cancellationToken = default);
}