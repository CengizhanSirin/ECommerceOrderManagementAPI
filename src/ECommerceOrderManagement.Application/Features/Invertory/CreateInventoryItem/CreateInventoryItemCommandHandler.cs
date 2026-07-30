using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Domain.Inventory;

namespace ECommerceOrderManagement.Application.Features.Invertory.CreateInventoryItem;

public sealed class CreateInventoryItemCommandHandler(IProductRepository productRepository, IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateInventoryItemCommand, CreateInventoryItemResponse>
{
    public async Task<Result<CreateInventoryItemResponse>> Handle(CreateInventoryItemCommand command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
        {
            return Result<CreateInventoryItemResponse>.Failure(InventoryErrors.ProductNotFound(command.ProductId));
        }

        var inventoryExists = await inventoryRepository.ExistsByProductIdAsync(command.ProductId, cancellationToken);

        if (inventoryExists)
        {
            return Result<CreateInventoryItemResponse>.Failure(InventoryErrors.AlreadyExists(command.ProductId));
        }

        var inventoryItem = InventoryItem.Create(
            command.ProductId,
            command.InitialQuantity,
            command.ReorderLevel);

        await inventoryRepository.AddAsync(inventoryItem, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateInventoryItemResponse>.Success(new CreateInventoryItemResponse
        {
            Id = inventoryItem.Id,
            ProductId = inventoryItem.ProductId
        });
    }
}