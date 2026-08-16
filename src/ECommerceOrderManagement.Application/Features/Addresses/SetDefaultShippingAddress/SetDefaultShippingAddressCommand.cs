using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Addresses.SetDefaultShippingAddress;

public sealed record SetDefaultShippingAddressCommand(Guid AddressId) : ICommand
{
}