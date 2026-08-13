namespace ECommerceOrderManagement.Application.Features.Addresses;

public interface IAddressQueries
{
    Task<AddressReadModel?> GetByIdAsync(Guid addressId,Guid userId,CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<AddressReadModel>> GetByUserIdAsync( Guid userId,CancellationToken cancellationToken = default);
}