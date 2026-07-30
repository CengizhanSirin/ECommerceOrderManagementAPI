using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Inventory.GetInventoryByProductId;
using ECommerceOrderManagement.Application.Features.Inventory.GetInventoryList;

namespace ECommerceOrderManagement.Application.Features.Inventory;

public interface IInventoryQueries
{
    Task<GetInventoryByProductIdResponse?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task<PagedResult<GetInventoryListItemResponse>> GetListAsync(GetInventoryListQuery query, CancellationToken cancellationToken = default);
}