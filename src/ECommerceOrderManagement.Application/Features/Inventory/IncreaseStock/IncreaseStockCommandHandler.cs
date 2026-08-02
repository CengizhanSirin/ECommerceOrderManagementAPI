using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Catalog.Products;

namespace ECommerceOrderManagement.Application.Features.Inventory.IncreaseStock;

public sealed class IncreaseStockCommandHandler(IProductRepository productRepository, IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<IncreaseStockCommand>
{
    public async Task<Result> Handle(IncreaseStockCommand command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure(InventoryErrors.ProductNotFound(command.ProductId));
        }

        var inventoryItem = await inventoryRepository.GetByProductIdAsync(command.ProductId, cancellationToken);

        if (inventoryItem is null)
        {
            return Result.Failure(InventoryErrors.NotFound(command.ProductId));
        }

        inventoryItem.IncreaseStock(command.Quantity, command.Reason);

        return await unitOfWork.SaveInventoryChangesAsync(command.ProductId, cancellationToken);
    }
}