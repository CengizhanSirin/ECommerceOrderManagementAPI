namespace ECommerceOrderManagement.API.Features.Inventory.IncreaseStock;

public sealed class IncreaseStockRequest
{
    public int Quantity { get; init; }

    public string? Reason { get; init; }
}