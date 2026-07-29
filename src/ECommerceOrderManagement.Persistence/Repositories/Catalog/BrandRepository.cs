using ECommerceOrderManagement.Application.Features.Catalog.Brands;
using ECommerceOrderManagement.Domain.Catalog;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Catalog;

internal sealed class BrandRepository(ApplicationDbContext dbContext) : Repository<Brand>(dbContext), IBrandRepository
{
    public Task<bool> ExistsByNameAsync(string name, Guid? excludedBrandId = null, CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim();

        return DbSet.AnyAsync(brand => brand.Name == normalizedName && (!excludedBrandId.HasValue || brand.Id != excludedBrandId.Value), cancellationToken);
    }

    public Task<bool> ExistsBySlugAsync(string slug, Guid? excludedBrandId = null, CancellationToken cancellationToken = default)
    {
        var normalizedSlug = slug.Trim().ToLowerInvariant();

        return DbSet.AnyAsync(brand => brand.Slug == normalizedSlug && (!excludedBrandId.HasValue || brand.Id != excludedBrandId.Value), cancellationToken);
    }
}