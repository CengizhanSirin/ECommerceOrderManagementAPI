using ECommerceOrderManagement.Application.Features.Inventory;
using ECommerceOrderManagement.Domain.Inventory;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Inventory;

internal sealed class InventoryRepository(ApplicationDbContext dbContext) : Repository<InventoryItem>(dbContext), IInventoryRepository
{
    public Task<InventoryItem?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return DbSet.SingleOrDefaultAsync(inventoryItem => inventoryItem.ProductId == productId, cancellationToken);
    }

    public Task<bool> ExistsByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return DbSet.AnyAsync(inventoryItem => inventoryItem.ProductId == productId, cancellationToken);
    }
}