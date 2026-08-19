using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Application.Features.Inventory;
using ECommerceOrderManagement.Domain.ShoppingCarts;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts.AddShoppingCartItem;

internal sealed class AddShoppingCartItemCommandHandler(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository, IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : ICommandHandler<AddShoppingCartItemCommand>
{
    public async Task<Result> Handle(AddShoppingCartItemCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        var product = await productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound(command.ProductId));
        }

        if (!product.IsActive)
        {
            return Result.Failure(ProductErrors.Inactive(command.ProductId));
        }

        var shoppingCart = await shoppingCartRepository.GetByUserIdAsync(userId, cancellationToken);

        var existingItem = shoppingCart?.Items.SingleOrDefault(item => item.ProductId == command.ProductId);

        var targetQuantity = (existingItem?.Quantity ?? 0) + command.Quantity;

        var inventoryItem = await inventoryRepository.GetByProductIdAsync(command.ProductId, cancellationToken);


        if (inventoryItem is null)
        {
            return Result.Failure(InventoryErrors.NotFound(command.ProductId));
        }

        if (targetQuantity > inventoryItem.AvailableQuantity)
        {
            return Result.Failure(InventoryErrors.InsufficientStock(targetQuantity, inventoryItem.AvailableQuantity));
        }

        if (shoppingCart is null)
        {
            shoppingCart = ShoppingCart.Create(userId);

            await shoppingCartRepository.AddAsync(shoppingCart, cancellationToken);
        }

        var itemAdded = shoppingCart.TryAddItem(command.ProductId, command.Quantity);


        if (!itemAdded)
        {
            return Result.Failure(ShoppingCartErrors.MaximumItemLimitReached());
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}