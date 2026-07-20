using ECommerceOrderManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerceOrderManagement.Persistence.Interceptors;

public sealed class AuditAndSoftDeleteInterceptor(TimeProvider timeProvider) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyChanges(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        ApplyChanges(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyChanges(DbContext? context)
    {
        if (context is null)
            return;

        var utcNow = timeProvider
            .GetUtcNow()
            .UtcDateTime;

        ApplyAuditInformation(context, utcNow);
        ApplySoftDelete(context, utcNow);
    }

    private static void ApplyAuditInformation(DbContext context, DateTime utcNow)
    {
        foreach (var entry in
                 context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(entity => entity.CreatedAtUtc)
                    .CurrentValue = utcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(entity => entity.UpdatedAtUtc)
                    .CurrentValue = utcNow;
            }
        }
    }

    private static void ApplySoftDelete(DbContext context, DateTime utcNow)
    {
        var deletedEntries = context.ChangeTracker
            .Entries<SoftDeletableAggregateRoot>()
            .Where(entry => entry.State == EntityState.Deleted);

        foreach (var entry in deletedEntries)
        {
            entry.State = EntityState.Modified;

            entry.Entity.Delete(utcNow);

            entry.Property(entity => entity.UpdatedAtUtc).CurrentValue = utcNow;
        }
    }
}
