namespace ECommerceOrderManagement.API.Features.Invertory.GetInventoryList;

public sealed class GetInventoryListRequest
{
    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 10;

    public string? Search { get; init; }

    public bool? IsLowStock { get; init; }

    public bool? IsOutOfStock { get; init; }

    public string SortBy { get; init; } = "productName";

    public string SortDirection { get; init; } = "asc";
}