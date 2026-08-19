namespace ECommerceOrderManagement.API.Features.ShoppingCarts.AddShoppingCartItem;

public sealed record AddShoppingCartItemRequest(Guid ProductId, int Quantity);