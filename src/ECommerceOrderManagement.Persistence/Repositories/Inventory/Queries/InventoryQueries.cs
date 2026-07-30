using ECommerceOrderManagement.Application.Features.Invertory;
using ECommerceOrderManagement.Application.Features.Invertory.GetInventoryByProductId;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Inventory.Queries;

public sealed class InventoryQueries(ApplicationDbContext dbContext) : IInventoryQueries
{
    public Task<GetInventoryByProductIdResponse?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return (from inventoryItem in dbContext.InventoryItems.AsNoTracking()
                join product in dbContext.Products.AsNoTracking()
                on inventoryItem.ProductId equals product.Id
                where inventoryItem.ProductId == productId
                select new GetInventoryByProductIdResponse
                {
                    Id = inventoryItem.Id,
                    ProductId = inventoryItem.ProductId,
                    ProductName = product.Name,
                    Sku = product.Sku,
                    QuantityOnHand = inventoryItem.QuantityOnHand,
                    ReservedQuantity = inventoryItem.ReservedQuantity,
                    AvailableQuantity = inventoryItem.QuantityOnHand - inventoryItem.ReservedQuantity,
                    ReorderLevel = inventoryItem.ReorderLevel,
                    IsLowStock = inventoryItem.QuantityOnHand - inventoryItem.ReservedQuantity <= inventoryItem.ReorderLevel,
                    IsOutOfStock = inventoryItem.QuantityOnHand - inventoryItem.ReservedQuantity == 0,
                    CreatedAtUtc = inventoryItem.CreatedAtUtc,
                    UpdatedAtUtc = inventoryItem.UpdatedAtUtc
                })
                 .SingleOrDefaultAsync(cancellationToken);
    }
}