namespace ECommerceOrderManagement.API.Features.Orders.CreateOrder;

public sealed record CreateOrderRequest(Guid ShippingAddressId, Guid BillingAddressId, string? CouponCode);