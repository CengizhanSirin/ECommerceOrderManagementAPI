namespace ECommerceOrderManagement.Application.Features.ShoppingCarts;

public sealed record ShoppingCartReadModel(Guid? Id, IReadOnlyCollection<ShoppingCartItemReadModel> Items, decimal Subtotal);