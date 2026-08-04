namespace ECommerceOrderManagement.API.Features.Orders.CreateOrder;

public sealed record CreateOrderAddressRequest(string FullName, string PhoneNumber, string Country, string City, string District, string? PostalCode, string AddressLine);