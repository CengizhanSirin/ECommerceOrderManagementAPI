namespace ECommerceOrderManagement.API.Features.Orders.CreateOrder;

public sealed record CreateOrderRequest(Guid ShippingAddressId, CreateOrderAddressRequest BillingAddress, IReadOnlyCollection<CreateOrderItemRequest> Items);