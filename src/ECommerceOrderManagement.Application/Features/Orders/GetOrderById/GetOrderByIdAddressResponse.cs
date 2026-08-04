namespace ECommerceOrderManagement.Application.Features.Orders.GetOrderById;

public sealed record GetOrderByIdAddressResponse(string FullName, string PhoneNumber, string Country, string City, string District, string? PostalCode, string AddressLine);