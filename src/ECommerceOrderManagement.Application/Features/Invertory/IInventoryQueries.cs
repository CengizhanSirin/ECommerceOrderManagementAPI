using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Invertory.GetInventoryByProductId;
using ECommerceOrderManagement.Application.Features.Invertory.GetInventoryList;

namespace ECommerceOrderManagement.Application.Features.Invertory;

public interface IInventoryQueries
{
    Task<GetInventoryByProductIdResponse?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task<PagedResult<GetInventoryListItemResponse>> GetListAsync(GetInventoryListQuery query, CancellationToken cancellationToken = default);
}