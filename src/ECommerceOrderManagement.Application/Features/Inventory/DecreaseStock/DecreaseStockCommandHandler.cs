using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Catalog.Products;

namespace ECommerceOrderManagement.Application.Features.Inventory.DecreaseStock;

public sealed class DecreaseStockCommandHandler(IProductRepository productRepository, IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<DecreaseStockCommand>
{
    public async Task<Result> Handle(DecreaseStockCommand command, CancellationToken cancellationToken)
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

        if (command.Quantity > inventoryItem.AvailableQuantity)
        {
            return Result.Failure(InventoryErrors.InsufficientStock(command.Quantity, inventoryItem.AvailableQuantity));
        }


        inventoryItem.DecreaseStock(command.Quantity,command.Reason);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}