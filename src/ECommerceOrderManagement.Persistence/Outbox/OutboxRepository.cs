using ECommerceOrderManagement.Application.Common.Abstractions.Outbox;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Outbox;

internal sealed class OutboxRepository(ApplicationDbContext dbContext) : IOutboxRepository
{
    public async Task<bool> MarkAsFailedAsync(Guid messageId, string error, CancellationToken cancellationToken = default)
    {
        var message = await dbContext.OutboxMessages
             .SingleOrDefaultAsync(message => message.Id == messageId, cancellationToken);

        if (message is null)
        {
            return false;
        }

        message.MarkAsFailed(error);

        return true;
    }

    public async Task<bool> MarkAsProcessedAsync(Guid messageId, DateTime processedOnUtc, CancellationToken cancellationToken = default)
    {
        var message = await dbContext.OutboxMessages
             .SingleOrDefaultAsync(message => message.Id == messageId, cancellationToken);

        if (message is null)
        {
            return false;
        }

        message.MarkAsProcessed(processedOnUtc);

        return true;
    }
}