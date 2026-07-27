using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Pagination;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products.GetProducts;

public sealed record GetProductsQuery(int PageNumber = 1, int PageSize = 20, string? SearchTerm = null, Guid? CategoryId = null, Guid? BrandId = null,
    bool? IsActive = null, ProductSortField SortBy = ProductSortField.CreatedAt, SortDirection SortDirection = SortDirection.Descending)
    : IQuery<PagedResult<GetProductsItemResponse>>
{
}