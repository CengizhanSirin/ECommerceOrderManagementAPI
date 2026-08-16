using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Addresses.GetAddresses;

public sealed record GetAddressesQuery : IQuery<IReadOnlyCollection<AddressReadModel>>
{
}