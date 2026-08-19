namespace ECommerceOrderManagement.Application.Features.ShoppingCarts;

public interface IShoppingCartQueries
{
    Task<ShoppingCartReadModel?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}