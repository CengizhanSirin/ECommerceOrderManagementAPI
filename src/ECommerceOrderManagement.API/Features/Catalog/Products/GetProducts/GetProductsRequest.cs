using ECommerceOrderManagement.Application.Features.Catalog.Products.GetProducts;

namespace ECommerceOrderManagement.API.Features.Catalog.Products.GetProducts;

public sealed class GetProductsRequest
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 20;

    public string? SearchTerm { get; init; }

    public Guid? CategoryId { get; init; }

    public Guid? BrandId { get; init; }

    public bool? IsActive { get; init; }

    public ProductSortField SortBy { get; init; } = ProductSortField.CreatedAt;

    public SortDirection SortDirection { get; init; } = SortDirection.Descending;
}