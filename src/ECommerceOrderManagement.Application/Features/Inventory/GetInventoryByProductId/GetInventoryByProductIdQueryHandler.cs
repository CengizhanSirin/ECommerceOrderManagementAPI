using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Inventory.GetInventoryByProductId;

public sealed class GetInventoryByProductIdQueryHandler(IInventoryQueries inventoryQueries) : IQueryHandler<GetInventoryByProductIdQuery, GetInventoryByProductIdResponse>
{
    public async Task<Result<GetInventoryByProductIdResponse>> Handle(GetInventoryByProductIdQuery query, CancellationToken cancellationToken)
    {
        var inventoryItem = await inventoryQueries.GetByProductIdAsync(query.ProductId, cancellationToken);

        if (inventoryItem is null)
        {
            return Result<GetInventoryByProductIdResponse>.Failure(InventoryErrors.NotFound(query.ProductId));
        }

        return Result<GetInventoryByProductIdResponse>.Success(inventoryItem);
    }
}