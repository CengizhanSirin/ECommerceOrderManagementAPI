using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Inventory;
using ECommerceOrderManagement.Application.Features.Inventory.GetInventoryByProductId;
using ECommerceOrderManagement.Application.Features.Inventory.GetInventoryList;
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

    public async Task<PagedResult<GetInventoryListItemResponse>> GetListAsync(GetInventoryListQuery query, CancellationToken cancellationToken = default)
    {
        var inventoryQuery =
            from inventoryItem in dbContext.InventoryItems.AsNoTracking()
            join product in dbContext.Products.AsNoTracking()
            on inventoryItem.ProductId equals product.Id
            select new GetInventoryListItemResponse
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
            };

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            inventoryQuery = inventoryQuery.Where(item => item.ProductName.Contains(search) || item.Sku.Contains(search));
        }

        if (query.IsLowStock.HasValue)
        {
            inventoryQuery = inventoryQuery.Where(item => item.IsLowStock == query.IsLowStock.Value);
        }

        if (query.IsOutOfStock.HasValue)
        {
            inventoryQuery = inventoryQuery.Where(item => item.IsOutOfStock == query.IsOutOfStock.Value);
        }

        var totalCount = await inventoryQuery.CountAsync(cancellationToken);


        var isDescending = query.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);


        IOrderedQueryable<GetInventoryListItemResponse> orderedQuery = (query.SortBy.ToLowerInvariant(), isDescending)
            switch
        {

            ("productname", false) => inventoryQuery.OrderBy(item => item.ProductName),

            ("productname", true) => inventoryQuery.OrderByDescending(item => item.ProductName),

            ("sku", false) => inventoryQuery.OrderBy(item => item.Sku),

            ("sku", true) => inventoryQuery.OrderByDescending(item => item.Sku),

            ("quantityonhand", false) => inventoryQuery.OrderBy(item => item.QuantityOnHand),

            ("quantityonhand", true) => inventoryQuery.OrderByDescending(item => item.QuantityOnHand),

            ("availablequantity", false) => inventoryQuery.OrderBy(item => item.AvailableQuantity),

            ("availablequantity", true) => inventoryQuery.OrderByDescending(item => item.AvailableQuantity),

            ("reorderlevel", false) => inventoryQuery.OrderBy(item => item.ReorderLevel),

            ("reorderlevel", true) => inventoryQuery.OrderByDescending(item => item.ReorderLevel),

            ("createdatutc", false) => inventoryQuery.OrderBy(item => item.CreatedAtUtc),

            ("createdatutc", true) => inventoryQuery.OrderByDescending(item => item.CreatedAtUtc),

            _ => inventoryQuery.OrderBy(item => item.ProductName)
        };

        var items = await orderedQuery
            .ThenBy(item => item.ProductId)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<GetInventoryListItemResponse>(
            items,
            query.Page,
            query.PageSize,
            totalCount);
    }
}