using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts;

public static class ShoppingCartErrors
{
    public static Error ItemNotFound(Guid productId)
    {
        return Error.NotFound("ShoppingCart.ItemNotFound", $"Product with ID '{productId}' was not found in the shopping cart.");
    }

    public static Error MaximumItemLimitReached()
    {
        return Error.Conflict("ShoppingCart.MaximumItemLimitReached", "The shopping cart cannot contain more than 50 different products.");
    }

    public static Error Empty()
    {
        return Error.Conflict("ShoppingCart.Empty", "An order cannot be created from an empty shopping cart.");
    }
}