using ECommerceOrderManagement.Domain.Inventory;

namespace ECommerceOrderManagement.API.Features.Inventory.GetStockMovementHistory;

public sealed class GetStockMovementHistoryRequest
{
    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 10;

    public StockMovementType? Type { get; init; }

    public string SortDirection { get; init; } = "desc";
}