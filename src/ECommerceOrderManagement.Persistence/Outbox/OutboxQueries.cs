using ECommerceOrderManagement.Application.Common.Abstractions.Outbox;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Outbox;

internal sealed class OutboxQueries(ApplicationDbContext dbContext) : IOutboxQueries
{
    public async Task<IReadOnlyList<OutboxMessageReadModel>> GetPendingAsync(int batchSize, int maxRetryCount, CancellationToken cancellationToken = default)
    {
        return await dbContext.OutboxMessages
            .AsNoTracking()
            .Where(message =>
                message.ProcessedOnUtc == null
                &&
                message.RetryCount < maxRetryCount)
            .OrderBy(message => message.OccurredOnUtc)
            .Take(batchSize)
            .Select(message => new OutboxMessageReadModel(
                message.Id,
                message.Type,
                message.Payload,
                message.OccurredOnUtc,
                message.RetryCount))
            .ToListAsync(cancellationToken);
    }
}