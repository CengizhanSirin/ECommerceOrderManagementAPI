using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Domain.ShoppingCarts;

public sealed class ShoppingCart : AggregateRoot
{
    private const int MaximumDistinctItems = 50;

    private readonly List<ShoppingCartItem> _items = [];

    public Guid UserId { get; private set; }

    public IReadOnlyCollection<ShoppingCartItem> Items => _items.AsReadOnly();

    private ShoppingCart()
    {
    }

    private ShoppingCart(Guid id, Guid userId) : base(id)
    {
        UserId = userId;
    }

    public static ShoppingCart Create(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User identifier cannot be empty.", nameof(userId));

        return new ShoppingCart(Guid.NewGuid(), userId);
    }

    public bool TryAddItem(Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("Product identifier cannot be empty.", nameof(productId));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

        var existingItem = _items.SingleOrDefault(item => item.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
            return true;
        }

        if (_items.Count >= MaximumDistinctItems)
            return false;

        var item = ShoppingCartItem.Create(Id, productId, quantity);

        _items.Add(item);

        return true;
    }

    public bool TryUpdateQuantity(Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("Product identifier cannot be empty.", nameof(productId));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

        var item = _items.SingleOrDefault(item => item.ProductId == productId);

        if (item is null)
            return false;

        item.ChangeQuantity(quantity);

        return true;
    }

    public bool TryRemoveItem(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("Product identifier cannot be empty.", nameof(productId));

        var item = _items.SingleOrDefault(item => item.ProductId == productId);

        if (item is null)
            return false;

        _items.Remove(item);

        return true;
    }

    public void Clear()
    {
        _items.Clear();
    }
}