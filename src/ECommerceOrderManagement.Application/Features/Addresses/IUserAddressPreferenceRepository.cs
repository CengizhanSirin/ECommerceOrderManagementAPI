using ECommerceOrderManagement.Domain.Addresses;

namespace ECommerceOrderManagement.Application.Features.Addresses;

public interface IUserAddressPreferenceRepository
{
    Task<UserAddressPreference?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task AddAsync(UserAddressPreference preference, CancellationToken cancellationToken = default);
}