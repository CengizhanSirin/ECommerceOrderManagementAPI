using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Addresses.SetDefaultAddress;

public sealed record SetDefaultAddressCommand(Guid AddressId) : ICommand
{
}