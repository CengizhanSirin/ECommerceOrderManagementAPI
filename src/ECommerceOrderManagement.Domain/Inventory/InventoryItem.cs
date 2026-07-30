using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Domain.Inventory;

public sealed class InventoryItem : AggregateRoot
{
    public Guid ProductId { get; private set; }

    public int QuantityOnHand { get; private set; }

    public int ReservedQuantity { get; private set; }

    public int ReorderLevel { get; private set; }

    public int AvailableQuantity => QuantityOnHand - ReservedQuantity;

    public bool IsLowStock => AvailableQuantity <= ReorderLevel;

    public bool IsOutOfStock => AvailableQuantity == 0;

    private InventoryItem()
    {
    }

    private InventoryItem(Guid productId, int initialQuantity, int reorderLevel)
    {
        ProductId = productId;
        QuantityOnHand = initialQuantity;
        ReservedQuantity = 0;
        ReorderLevel = reorderLevel;
    }

    public static InventoryItem Create(Guid productId, int initialQuantity, int reorderLevel)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        }

        EnsureNonNegativeQuantity(initialQuantity, nameof(initialQuantity));

        EnsureNonNegativeQuantity(reorderLevel, nameof(reorderLevel));

        return new InventoryItem(productId, initialQuantity, reorderLevel);
    }

    public void IncreaseStock(int quantity)
    {
        EnsurePositiveQuantity(quantity);

        QuantityOnHand += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        EnsurePositiveQuantity(quantity);

        if (quantity > AvailableQuantity)
        {
            throw new InvalidOperationException("Stock cannot be decreased below the reserved quantity.");
        }

        QuantityOnHand -= quantity;
    }

    public void ChangeReorderLevel(int reorderLevel)
    {
        EnsureNonNegativeQuantity(reorderLevel, nameof(reorderLevel));

        ReorderLevel = reorderLevel;
    }

    private static void EnsurePositiveQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }
    }

    private static void EnsureNonNegativeQuantity(int quantity, string parameterName)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Quantity cannot be negative.");
        }
    }
}