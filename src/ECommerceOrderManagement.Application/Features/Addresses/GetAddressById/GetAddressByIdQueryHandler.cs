using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Addresses.GetAddressById;

internal sealed class GetAddressByIdQueryHandler(IAddressQueries addressQueries, ICurrentUser currentUser) : IQueryHandler<GetAddressByIdQuery, AddressReadModel>
{
    public async Task<Result<AddressReadModel>> Handle(GetAddressByIdQuery query, CancellationToken cancellationToken)
    {
        var address = await addressQueries.GetByIdAsync(query.AddressId, currentUser.UserId, cancellationToken);

        if (address is null)
        {
            return Result<AddressReadModel>.Failure(AddressErrors.NotFound(query.AddressId));
        }

        return Result<AddressReadModel>.Success(address);
    }
}