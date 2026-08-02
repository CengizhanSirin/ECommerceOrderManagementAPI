using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Catalog.Products;

namespace ECommerceOrderManagement.Application.Features.Inventory.ChangeReorderLevel;

public sealed class ChangeReorderLevelCommandHandler(IProductRepository productRepository, IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<ChangeReorderLevelCommand>
{
    public async Task<Result> Handle(ChangeReorderLevelCommand command, CancellationToken cancellationToken)
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

        inventoryItem.ChangeReorderLevel(command.ReorderLevel);

        return await unitOfWork.SaveInventoryChangesAsync(command.ProductId, cancellationToken);
    }
}