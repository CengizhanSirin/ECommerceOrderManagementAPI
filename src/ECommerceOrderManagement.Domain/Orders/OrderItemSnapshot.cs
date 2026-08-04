namespace ECommerceOrderManagement.Domain.Orders;

public sealed record OrderItemSnapshot(Guid ProductId, string ProductName, string Sku, decimal UnitPrice, int Quantity);