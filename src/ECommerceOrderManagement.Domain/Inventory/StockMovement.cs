using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Domain.Inventory;

public sealed class StockMovement : AuditableEntity
{
    public Guid InventoryItemId { get; private set; }

    public Guid ProductId { get; private set; }

    public StockMovementType Type { get; private set; }

    public int Quantity { get; private set; }

    public int QuantityOnHandBefore { get; private set; }

    public int QuantityOnHandAfter { get; private set; }

    public int ReservedQuantityBefore { get; private set; }

    public int ReservedQuantityAfter { get; private set; }

    public string? Reason { get; private set; }

    private StockMovement()
    {
    }

    private StockMovement(Guid inventoryItemId, Guid productId, StockMovementType type,
        int quantity, int quantityOnHandBefore, int quantityOnHandAfter,
        int reservedQuantityBefore, int reservedQuantityAfter, string? reason)
    {
        InventoryItemId = inventoryItemId;
        ProductId = productId;
        Type = type;
        Quantity = quantity;
        QuantityOnHandBefore = quantityOnHandBefore;
        QuantityOnHandAfter = quantityOnHandAfter;
        ReservedQuantityBefore = reservedQuantityBefore;
        ReservedQuantityAfter = reservedQuantityAfter;
        Reason = string.IsNullOrWhiteSpace(reason)
            ? null
            : reason.Trim();
    }

    internal static StockMovement Create(Guid inventoryItemId, Guid productId, StockMovementType type,
        int quantity, int quantityOnHandBefore, int quantityOnHandAfter,
        int reservedQuantityBefore, int reservedQuantityAfter, string? reason)
    {
        if (inventoryItemId == Guid.Empty)
        {
            throw new ArgumentException("Inventory item ID cannot be empty.", nameof(inventoryItemId));
        }

        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        }

        if (!Enum.IsDefined(type))
        {
            throw new ArgumentOutOfRangeException(nameof(type), "Stock movement type is invalid.");
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        EnsureValidStockSnapshot(quantityOnHandBefore, reservedQuantityBefore);

        EnsureValidStockSnapshot(quantityOnHandAfter, reservedQuantityAfter);

        return new StockMovement(inventoryItemId, productId, type, quantity, quantityOnHandBefore, quantityOnHandAfter,
            reservedQuantityBefore, reservedQuantityAfter, reason);
    }

    private static void EnsureValidStockSnapshot(int quantityOnHand, int reservedQuantity)
    {
        if (quantityOnHand < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantityOnHand), "Quantity on hand cannot be negative.");
        }

        if (reservedQuantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(reservedQuantity), "Reserved quantity cannot be negative.");
        }

        if (reservedQuantity > quantityOnHand)
        {
            throw new InvalidOperationException("Reserved quantity cannot exceed quantity on hand.");
        }
    }
}