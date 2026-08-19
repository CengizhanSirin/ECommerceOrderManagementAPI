using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Application.Features.Inventory;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts.UpdateShoppingCartItem;

internal sealed class UpdateShoppingCartItemCommandHandler(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository,
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateShoppingCartItemCommand>
{
    public async Task<Result> Handle(UpdateShoppingCartItemCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        var shoppingCart = await shoppingCartRepository.GetByUserIdAsync(userId, cancellationToken);

        if (shoppingCart is null)
        {
            return Result.Failure(ShoppingCartErrors.ItemNotFound(command.ProductId));
        }

        var existingItem = shoppingCart.Items.SingleOrDefault(item => item.ProductId == command.ProductId);

        if (existingItem is null)
        {
            return Result.Failure(ShoppingCartErrors.ItemNotFound(command.ProductId));
        }

        var product = await productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound(command.ProductId));
        }

        if (!product.IsActive)
        {
            return Result.Failure(ProductErrors.Inactive(command.ProductId));
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

        var updated = shoppingCart.TryUpdateQuantity(command.ProductId, command.Quantity);

        if (!updated)
        {
            return Result.Failure(ShoppingCartErrors.ItemNotFound(command.ProductId));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}