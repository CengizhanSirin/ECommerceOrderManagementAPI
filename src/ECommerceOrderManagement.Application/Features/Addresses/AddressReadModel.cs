namespace ECommerceOrderManagement.Application.Features.Addresses;

public sealed record AddressReadModel(Guid Id, string Title, string FullName, string PhoneNumber, string Country, string City, string District, string PostalCode,
    string AddressLine, bool IsDefault);