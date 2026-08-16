using ECommerceOrderManagement.Application.Features.Addresses;
using ECommerceOrderManagement.Domain.Addresses;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Addresses;

internal sealed class UserAddressPreferenceRepository(ApplicationDbContext dbContext) : IUserAddressPreferenceRepository
{
    public async Task<UserAddressPreference?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.UserAddressPreferences.SingleOrDefaultAsync(preference => preference.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(UserAddressPreference preference, CancellationToken cancellationToken = default)
    {
        await dbContext.UserAddressPreferences.AddAsync(preference, cancellationToken);
    }
}