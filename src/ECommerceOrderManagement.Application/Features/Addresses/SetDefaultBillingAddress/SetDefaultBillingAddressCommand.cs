using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Addresses.SetDefaultBillingAddress;

public sealed record SetDefaultBillingAddressCommand(Guid AddressId) : ICommand
{
}