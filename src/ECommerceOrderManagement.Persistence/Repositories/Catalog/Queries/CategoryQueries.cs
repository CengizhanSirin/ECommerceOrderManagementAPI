using ECommerceOrderManagement.Application.Features.Catalog.Categories;
using ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategoryById;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Catalog.Queries;

internal sealed class CategoryQueries(ApplicationDbContext dbContext) : ICategoryQueries
{
    public Task<GetCategoryByIdResponse?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return dbContext.Categories
             .AsNoTracking()
             .Where(category => category.Id == categoryId)
             .Select(category => new GetCategoryByIdResponse
             {
                 Id = category.Id,
                 Name = category.Name,
                 Slug = category.Slug,
                 Description = category.Description,
                 IsActive = category.IsActive,
                 ImageUrl = category.ImageUrl,
                 DisplayOrder = category.DisplayOrder,
                 CreatedAtUtc = category.CreatedAtUtc,
                 UpdatedAtUtc = category.UpdatedAtUtc
             })
             .SingleOrDefaultAsync(cancellationToken);
    }
}