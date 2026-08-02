using ECommerceOrderManagement.Domain.Inventory;

namespace ECommerceOrderManagement.Application.Features.Inventory.GetStockMovementHistory;

public sealed record GetStockMovementHistoryItemResponse
{
    public Guid Id { get; init; }

    public Guid InventoryItemId { get; init; }

    public Guid ProductId { get; init; }

    public StockMovementType Type { get; init; }

    public int Quantity { get; init; }

    public int QuantityOnHandBefore { get; init; }

    public int QuantityOnHandAfter { get; init; }

    public int ReservedQuantityBefore { get; init; }

    public int ReservedQuantityAfter { get; init; }

    public int AvailableQuantityBefore { get; init; }

    public int AvailableQuantityAfter { get; init; }

    public string? Reason { get; init; }

    public DateTime CreatedAtUtc { get; init; }
}