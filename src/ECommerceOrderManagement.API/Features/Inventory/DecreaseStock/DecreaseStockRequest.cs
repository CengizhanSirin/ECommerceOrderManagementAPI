namespace ECommerceOrderManagement.API.Features.Inventory.DecreaseStock;

public sealed class DecreaseStockRequest
{
    public int Quantity { get; init; }

    public string? Reason { get; init; }
}