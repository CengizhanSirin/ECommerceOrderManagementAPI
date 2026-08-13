using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Domain.Addresses;

namespace ECommerceOrderManagement.Application.Features.Addresses;

public interface IAddressRepository : IRepository<Address>
{
    Task<Address?> GetByIdAndUserIdAsync(Guid addressId, Guid userId, CancellationToken cancellationToken = default);

    Task<Address?> GetDefaultByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> ExistsForUserAsync(Guid userId, CancellationToken cancellationToken = default);
}