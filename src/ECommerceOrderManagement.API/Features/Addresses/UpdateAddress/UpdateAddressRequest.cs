namespace ECommerceOrderManagement.API.Features.Addresses.UpdateAddress;

public sealed class UpdateAddressRequest
{
    public string Title { get; init; } = string.Empty;

    public string FullName { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string Country { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string District { get; init; } = string.Empty;

    public string PostalCode { get; init; } = string.Empty;

    public string AddressLine { get; init; } = string.Empty;
}