using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts.GetShoppingCart;

internal sealed class GetShoppingCartQueryHandler(IShoppingCartQueries shoppingCartQueries, ICurrentUser currentUser) : IQueryHandler<GetShoppingCartQuery, ShoppingCartReadModel>
{
    public async Task<Result<ShoppingCartReadModel>> Handle(GetShoppingCartQuery query, CancellationToken cancellationToken)
    {
        var cart = await shoppingCartQueries.GetByUserIdAsync(currentUser.UserId, cancellationToken);

        if (cart is not null)
        {
            return Result<ShoppingCartReadModel>.Success(cart);
        }

        var emptyCart = new ShoppingCartReadModel(null, [], 0m);

        return Result<ShoppingCartReadModel>.Success(emptyCart);
    }
}