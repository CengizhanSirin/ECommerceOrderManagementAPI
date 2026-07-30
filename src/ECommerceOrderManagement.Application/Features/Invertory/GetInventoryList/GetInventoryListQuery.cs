using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Pagination;

namespace ECommerceOrderManagement.Application.Features.Invertory.GetInventoryList;

public sealed record GetInventoryListQuery(int Page = 1, int PageSize = 10, string? Search = null, bool? IsLowStock = null,
    bool? IsOutOfStock = null, string SortBy = "productName", string SortDirection = "asc")
    : IQuery<PagedResult<GetInventoryListItemResponse>>
{
}