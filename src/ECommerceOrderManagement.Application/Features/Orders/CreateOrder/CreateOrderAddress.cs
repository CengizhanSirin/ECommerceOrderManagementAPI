namespace ECommerceOrderManagement.Application.Features.Orders.CreateOrder;

public sealed record CreateOrderAddress(string FullName, string PhoneNumber, string Country, string City, string District, string? PostalCode, string AddressLine);