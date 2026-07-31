namespace ECommerceOrderManagement.API.Features.Inventory.ReleaseStock;

public sealed class ReleaseStockRequest
{
    public int Quantity { get; init; }

    public string? Reason { get; init; }
}