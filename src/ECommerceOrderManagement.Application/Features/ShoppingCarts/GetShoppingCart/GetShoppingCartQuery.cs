using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts.GetShoppingCart;

public sealed record GetShoppingCartQuery : IQuery<ShoppingCartReadModel>;