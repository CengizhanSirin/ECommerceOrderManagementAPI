using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Addresses.GetAddressById;

public sealed record GetAddressByIdQuery(Guid AddressId) : IQuery<AddressReadModel>;