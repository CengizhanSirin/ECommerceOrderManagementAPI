using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Catalog.Products;

namespace ECommerceOrderManagement.Application.Features.Inventory.GetStockMovementHistory;

public sealed class GetStockMovementHistoryQueryHandler(IProductRepository productRepository, IInventoryRepository inventoryRepository, IInventoryQueries inventoryQueries)
    : IQueryHandler<GetStockMovementHistoryQuery, PagedResult<GetStockMovementHistoryItemResponse>>
{
    public async Task<Result<PagedResult<GetStockMovementHistoryItemResponse>>> Handle(GetStockMovementHistoryQuery query, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(query.ProductId, cancellationToken);

        if (product is null)
        {
            return Result<PagedResult<GetStockMovementHistoryItemResponse>>.Failure(InventoryErrors.ProductNotFound(query.ProductId));
        }

        var inventoryExists = await inventoryRepository.ExistsByProductIdAsync(query.ProductId, cancellationToken);

        if (!inventoryExists)
        {
            return Result<PagedResult<GetStockMovementHistoryItemResponse>>.Failure(InventoryErrors.NotFound(query.ProductId));
        }

        var stockMovements = await inventoryQueries.GetStockMovementHistoryAsync(query, cancellationToken);

        return Result<PagedResult<GetStockMovementHistoryItemResponse>>.Success(stockMovements);
    }
}