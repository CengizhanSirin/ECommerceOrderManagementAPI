using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Addresses.CreateAddress;

public sealed record CreateAddressCommand(string Title, string FullName, string PhoneNumber, string Country, string City, string District, string PostalCode,
    string AddressLine, bool IsDefault) : ICommand<Guid>;