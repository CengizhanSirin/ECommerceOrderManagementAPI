using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Invertory.GetInventoryList;

public sealed class GetInventoryListQueryHandler(IInventoryQueries inventoryQueries) : IQueryHandler<GetInventoryListQuery, PagedResult<GetInventoryListItemResponse>>
{
    public async Task<Result<PagedResult<GetInventoryListItemResponse>>> Handle(GetInventoryListQuery request, CancellationToken cancellationToken)
    {
        var inventoryItems = await inventoryQueries.GetListAsync(request, cancellationToken);

        return Result<PagedResult<GetInventoryListItemResponse>>.Success(inventoryItems);
    }
}