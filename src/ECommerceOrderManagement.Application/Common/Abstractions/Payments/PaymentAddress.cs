namespace ECommerceOrderManagement.Application.Common.Abstractions.Payments;

public sealed record PaymentAddress(string FullName, string PhoneNumber, string Country, string City, string District, string PostalCode, string AddressLine);