using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Application.Common.Abstractions.Persistence;

public interface IRepository<TAggregate> where TAggregate : AggregateRoot
{
    Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
}