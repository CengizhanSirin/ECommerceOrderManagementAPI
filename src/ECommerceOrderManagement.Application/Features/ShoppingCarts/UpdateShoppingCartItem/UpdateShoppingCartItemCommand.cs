using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts.UpdateShoppingCartItem;

public sealed record UpdateShoppingCartItemCommand(Guid ProductId, int Quantity) : ICommand;