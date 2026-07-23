using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Domain.Catalog;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Catalog;

internal sealed class ProductRepository(ApplicationDbContext dbContext) : Repository<Product>(dbContext), IProductRepository
{
    public Task<bool> ExistsBySkuAsync(string sku, Guid? excludedProductId = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);

        var normalizedSku = sku.Trim().ToUpperInvariant();

        return DbSet.AsNoTracking().AnyAsync(
            product =>
            product.Sku == normalizedSku && (!excludedProductId.HasValue || product.Id != excludedProductId.Value), cancellationToken);
    }

    public Task<bool> ExistsBySlugAsync(string slug, Guid? excludedProductId = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        var normalizedSlug = slug.Trim().ToLowerInvariant();

        return DbSet.AsNoTracking().AnyAsync(
            product =>
            product.Slug == normalizedSlug && (!excludedProductId.HasValue || product.Id != excludedProductId.Value), cancellationToken);
    }
}