namespace ECommerceOrderManagement.Application.Features.ShoppingCarts;

public sealed record ShoppingCartItemReadModel(Guid ProductId, string ProductName, string Sku, decimal UnitPrice, int Quantity, decimal LineTotal,
    int AvailableQuantity, bool IsAvailable);