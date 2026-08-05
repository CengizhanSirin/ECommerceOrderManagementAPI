namespace ECommerceOrderManagement.Application.Features.Orders.GetOrderById;

public sealed record GetOrderByIdItemResponse(Guid Id, Guid ProductId, string ProductName, string Sku, decimal UnitPrice, int Quantity, decimal LineTotal);