namespace ECommerceOrderManagement.API.Features.Orders.CreateOrder;

public sealed record CreateOrderItemRequest(Guid ProductId, int Quantity);