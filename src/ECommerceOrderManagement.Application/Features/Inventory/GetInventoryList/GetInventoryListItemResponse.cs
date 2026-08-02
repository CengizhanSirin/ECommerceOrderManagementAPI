namespace ECommerceOrderManagement.Application.Features.Inventory.GetInventoryList;

public sealed record GetInventoryListItemResponse
{
    public Guid Id { get; init; }

    public Guid ProductId { get; init; }

    public string ProductName { get; init; } = string.Empty;

    public string Sku { get; init; } = string.Empty;

    public int QuantityOnHand { get; init; }

    public int ReservedQuantity { get; init; }

    public int AvailableQuantity { get; init; }

    public int ReorderLevel { get; init; }

    public bool IsLowStock { get; init; }

    public bool IsOutOfStock { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }
}