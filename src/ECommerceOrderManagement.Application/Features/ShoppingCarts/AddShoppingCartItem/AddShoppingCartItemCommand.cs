using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts.AddShoppingCartItem;

public sealed record AddShoppingCartItemCommand(Guid ProductId, int Quantity) : ICommand;