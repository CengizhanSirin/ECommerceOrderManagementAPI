using ECommerceOrderManagement.Application.Features.Addresses;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Addresses.Queries;

internal sealed class AddressQueries(ApplicationDbContext dbContext) : IAddressQueries
{
    public async Task<AddressReadModel?> GetByIdAsync(Guid addressId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Addresses
            .AsNoTracking()
            .Where(address => address.Id == addressId && address.UserId == userId)
            .Select(address => new AddressReadModel(
                address.Id,
                address.Title,
                address.FullName,
                address.PhoneNumber,
                address.Country,
                address.City,
                address.District,
                address.PostalCode,
                address.AddressLine))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<AddressReadModel>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Addresses
           .AsNoTracking()
           .Where(address => address.UserId == userId)
           .OrderBy(address => address.Title)
           .Select(address => new AddressReadModel(
               address.Id,
               address.Title,
               address.FullName,
               address.PhoneNumber,
               address.Country,
               address.City,
               address.District,
               address.PostalCode,
               address.AddressLine))
           .ToArrayAsync(cancellationToken);
    }
}