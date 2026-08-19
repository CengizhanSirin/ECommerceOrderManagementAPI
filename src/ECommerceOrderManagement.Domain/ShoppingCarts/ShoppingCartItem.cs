using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Domain.ShoppingCarts;

public sealed class ShoppingCartItem : AuditableEntity
{
    public Guid ShoppingCartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    private ShoppingCartItem()
    {
    }

    private ShoppingCartItem(Guid id, Guid shoppingCartId, Guid productId, int quantity) : base(id)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
    }

    internal static ShoppingCartItem Create(Guid shoppingCartId, Guid productId, int quantity)
    {
        if (shoppingCartId == Guid.Empty) throw new ArgumentException("Shopping cart identifier cannot be empty.", nameof(shoppingCartId));

        if (productId == Guid.Empty) throw new ArgumentException("Product identifier cannot be empty.", nameof(productId));

        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

        return new ShoppingCartItem(Guid.NewGuid(), shoppingCartId, productId, quantity);
    }

    internal void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

        Quantity += quantity;
    }

    internal void ChangeQuantity(int quantity)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

        Quantity = quantity;
    }
}