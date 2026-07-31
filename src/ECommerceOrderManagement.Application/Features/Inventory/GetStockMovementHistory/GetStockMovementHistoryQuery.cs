using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Domain.Inventory;

namespace ECommerceOrderManagement.Application.Features.Inventory.GetStockMovementHistory;

public sealed record GetStockMovementHistoryQuery(Guid ProductId, int Page = 1, int PageSize = 10, StockMovementType? Type = null,
    string SortDirection = "desc") : IQuery<PagedResult<GetStockMovementHistoryItemResponse>>
{
}