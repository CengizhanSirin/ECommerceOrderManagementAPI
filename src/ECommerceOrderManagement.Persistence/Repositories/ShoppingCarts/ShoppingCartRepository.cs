using ECommerceOrderManagement.Application.Features.ShoppingCarts;
using ECommerceOrderManagement.Domain.ShoppingCarts;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.ShoppingCarts;

internal sealed class ShoppingCartRepository(ApplicationDbContext dbContext) : Repository<ShoppingCart>(dbContext), IShoppingCartRepository
{
    public Task<ShoppingCart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return DbSet.Include(cart => cart.Items).SingleOrDefaultAsync(cart => cart.UserId == userId, cancellationToken);
    }
}