using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Addresses.GetAddresses;

internal sealed class GetAddressesQueryHandler(IAddressQueries addressQueries, ICurrentUser currentUser) : IQueryHandler<GetAddressesQuery, IReadOnlyCollection<AddressReadModel>>
{
    public async Task<Result<IReadOnlyCollection<AddressReadModel>>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
    {
        var addresses = await addressQueries.GetByUserIdAsync(currentUser.UserId, cancellationToken);

        return Result<IReadOnlyCollection<AddressReadModel>>.Success(addresses);
    }
}