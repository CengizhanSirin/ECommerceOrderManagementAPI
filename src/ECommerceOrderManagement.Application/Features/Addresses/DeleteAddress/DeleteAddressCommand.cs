using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Addresses.DeleteAddress;

public sealed record DeleteAddressCommand(Guid AddressId) : ICommand
{
}