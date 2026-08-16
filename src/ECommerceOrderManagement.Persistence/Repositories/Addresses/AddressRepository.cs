using ECommerceOrderManagement.Application.Features.Addresses;
using ECommerceOrderManagement.Domain.Addresses;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Addresses;

internal sealed class AddressRepository(ApplicationDbContext dbContext) : Repository<Address>(dbContext), IAddressRepository
{
    public async Task<Address?> GetByIdAndUserIdAsync(Guid addressId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.SingleOrDefaultAsync(address => address.Id == addressId && address.UserId == userId, cancellationToken);
    }
}