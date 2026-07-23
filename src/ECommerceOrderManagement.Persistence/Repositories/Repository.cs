using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Domain.Common;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories;

internal abstract class Repository<TAggregate> : IRepository<TAggregate> where TAggregate : AggregateRoot
{
    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TAggregate>();
    }

    protected ApplicationDbContext DbContext { get; }

    protected DbSet<TAggregate> DbSet { get; }

    public Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbSet.SingleOrDefaultAsync(aggregate => aggregate.Id == id, cancellationToken);
    }

    public async Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        await DbSet.AddAsync(aggregate, cancellationToken);
    }
}