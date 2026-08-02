namespace ECommerceOrderManagement.API.Features.Inventory.ReserveStock;

public sealed class ReserveStockRequest
{
    public int Quantity { get; init; }

    public string? Reason { get; init; }
}