using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Inventory.GetInventoryByProductId;
using ECommerceOrderManagement.Application.Features.Inventory.GetInventoryList;
using ECommerceOrderManagement.Application.Features.Inventory.GetStockMovementHistory;

namespace ECommerceOrderManagement.Application.Features.Inventory;

public interface IInventoryQueries
{
    Task<GetInventoryByProductIdResponse?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task<PagedResult<GetInventoryListItemResponse>> GetListAsync(GetInventoryListQuery query, CancellationToken cancellationToken = default);

    Task<PagedResult<GetStockMovementHistoryItemResponse>> GetStockMovementHistoryAsync(GetStockMovementHistoryQuery query, CancellationToken cancellationToken = default);
}