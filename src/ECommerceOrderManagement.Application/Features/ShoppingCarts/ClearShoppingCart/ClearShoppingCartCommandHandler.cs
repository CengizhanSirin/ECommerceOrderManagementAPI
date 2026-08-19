using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts.ClearShoppingCart;

internal sealed class ClearShoppingCartCommandHandler(IShoppingCartRepository shoppingCartRepository,IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : ICommandHandler<ClearShoppingCartCommand>
{
    public async Task<Result> Handle( ClearShoppingCartCommand command,CancellationToken cancellationToken)
    {
        var shoppingCart = await shoppingCartRepository.GetByUserIdAsync(currentUser.UserId,cancellationToken);

        if (shoppingCart is null)
        {
            return Result.Success();
        }

        if (shoppingCart.Items.Count == 0)
        {
            return Result.Success();
        }

        shoppingCart.Clear();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}