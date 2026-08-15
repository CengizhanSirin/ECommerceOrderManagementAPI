using ECommerceOrderManagement.Application.Features.Addresses;
using ECommerceOrderManagement.Domain.Addresses;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Addresses;

internal sealed class AddressRepository(ApplicationDbContext dbContext) : Repository<Address>(dbContext), IAddressRepository
{
    public async Task<bool> ExistsForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(address => address.UserId == userId, cancellationToken);
    }

    public async Task<Address?> GetAnotherByUserIdAsync(Guid userId, Guid excludedAddressId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(address => address.UserId == userId && address.Id != excludedAddressId)
            .OrderBy(address => address.CreatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Address?> GetByIdAndUserIdAsync(Guid addressId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.SingleOrDefaultAsync(address => address.Id == addressId && address.UserId == userId, cancellationToken);
    }

    public async Task<Address?> GetDefaultByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.SingleOrDefaultAsync(address => address.UserId == userId && address.IsDefault, cancellationToken);
    }
}