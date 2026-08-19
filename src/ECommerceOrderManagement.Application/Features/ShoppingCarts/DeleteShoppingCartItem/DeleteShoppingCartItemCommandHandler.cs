using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts.DeleteShoppingCartItem;

internal sealed class DeleteShoppingCartItemCommandHandler(IShoppingCartRepository shoppingCartRepository, IUnitOfWork unitOfWork,
    ICurrentUser currentUser)
    : ICommandHandler<DeleteShoppingCartItemCommand>
{
    public async Task<Result> Handle(DeleteShoppingCartItemCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await shoppingCartRepository.GetByUserIdAsync(currentUser.UserId, cancellationToken);

        if (shoppingCart is null)
        {
            return Result.Failure(ShoppingCartErrors.ItemNotFound(command.ProductId));
        }

        var removed = shoppingCart.TryRemoveItem(command.ProductId);

        if (!removed)
        {
            return Result.Failure(ShoppingCartErrors.ItemNotFound(command.ProductId));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}