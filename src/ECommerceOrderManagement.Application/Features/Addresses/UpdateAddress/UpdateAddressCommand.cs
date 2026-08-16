using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Addresses.UpdateAddress;

public sealed record UpdateAddressCommand(Guid AddressId, string Title, string FullName, string PhoneNumber, string Country, string City, string District, string PostalCode,
    string AddressLine) : ICommand
{ }