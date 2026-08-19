using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts.DeleteShoppingCartItem;

public sealed record DeleteShoppingCartItemCommand(Guid ProductId) : ICommand;