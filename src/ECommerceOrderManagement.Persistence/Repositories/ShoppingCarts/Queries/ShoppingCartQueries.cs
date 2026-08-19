using ECommerceOrderManagement.Application.Features.ShoppingCarts;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.ShoppingCarts.Queries;

internal sealed class ShoppingCartQueries(ApplicationDbContext dbContext) : IShoppingCartQueries
{
    public async Task<ShoppingCartReadModel?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var cartId = await dbContext.ShoppingCarts
            .AsNoTracking()
            .Where(cart => cart.UserId == userId)
            .Select(cart => (Guid?)cart.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (cartId is null)
        {
            return null;
        }

        var items = await dbContext.ShoppingCartItems.AsNoTracking().Where(cartİtems => cartİtems.ShoppingCartId == cartId.Value)
            .GroupJoin(dbContext.Products
            .IgnoreQueryFilters()
            .AsNoTracking(),
            cartİtems => cartİtems.ProductId,
            p => p.Id,
            (cartİtems, products) => new
            {
                cartİtems,
                product = products.FirstOrDefault()
            })

            .Select(x => new
            {
                x.cartİtems,
                x.product,
                AvailableQuantity = dbContext.InventoryItems.AsNoTracking()
                .Where(inv => inv.ProductId == x.cartİtems.ProductId)
                .Select(inv => inv.QuantityOnHand - inv.ReservedQuantity)
                .FirstOrDefault()
            })

            .Select(x => new ShoppingCartItemReadModel(
                x.cartİtems.ProductId,
                x.product != null ? x.product.Name : string.Empty,
                x.product != null ? x.product.Sku : string.Empty,
                x.product != null ? x.product.Price.Amount : 0m,
                x.cartİtems.Quantity,
                (x.product != null ? x.product.Price.Amount : 0m) * x.cartİtems.Quantity,
                x.AvailableQuantity,
                x.product != null && !x.product.IsDeleted && x.product.IsActive && x.AvailableQuantity >= x.cartİtems.Quantity))
            .ToListAsync(cancellationToken);


        var subtotal = items.Sum(item => item.LineTotal);

        return new ShoppingCartReadModel(cartId.Value, items, subtotal);
    }
}