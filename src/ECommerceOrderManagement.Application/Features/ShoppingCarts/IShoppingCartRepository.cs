using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Domain.ShoppingCarts;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    Task<ShoppingCart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}