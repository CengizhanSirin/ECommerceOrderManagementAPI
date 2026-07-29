using ECommerceOrderManagement.Application.Features.Catalog.Categories;
using ECommerceOrderManagement.Domain.Catalog;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Catalog;

internal sealed class CategoryRepository(ApplicationDbContext dbContext) : Repository<Category>(dbContext), ICategoryRepository
{
    public Task<bool> ExistsByNameAsync(string name, Guid? excludedCategoryId = null, CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim();

        return DbSet.AnyAsync(category =>
                category.Name == normalizedName && (!excludedCategoryId.HasValue || category.Id != excludedCategoryId.Value), cancellationToken);
    }

    public Task<bool> ExistsBySlugAsync(string slug, Guid? excludedCategoryId = null, CancellationToken cancellationToken = default)
    {
        var normalizedSlug = slug.Trim().ToLowerInvariant();

        return DbSet.AnyAsync(category =>
                category.Slug == normalizedSlug && (!excludedCategoryId.HasValue || category.Id != excludedCategoryId.Value), cancellationToken);
    }
}