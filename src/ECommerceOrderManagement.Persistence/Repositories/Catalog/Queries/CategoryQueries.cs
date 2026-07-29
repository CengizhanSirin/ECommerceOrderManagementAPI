using ECommerceOrderManagement.Application.Features.Catalog.Categories;
using ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategories;
using ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategoryById;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Catalog.Queries;

internal sealed class CategoryQueries(ApplicationDbContext dbContext) : ICategoryQueries
{
    public async Task<IReadOnlyCollection<GetCategoriesItemResponse>> GetAllAsync(GetCategoriesQuery query, CancellationToken cancellationToken = default)
    {
        var categoriesQuery = dbContext.Categories.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.Trim();

            categoriesQuery = categoriesQuery.Where(category => category.Name.Contains(searchTerm) || category.Slug.Contains(searchTerm));
        }

        if (query.IsActive.HasValue)
        {
            categoriesQuery = categoriesQuery.Where(category => category.IsActive == query.IsActive.Value);
        }

        return await categoriesQuery
            .OrderBy(category => category.DisplayOrder)
            .ThenBy(category => category.Name)
            .ThenBy(category => category.Id)
            .Select(category => new GetCategoriesItemResponse
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                IsActive = category.IsActive,
                ImageUrl = category.ImageUrl,
                DisplayOrder = category.DisplayOrder
            })
            .ToArrayAsync(cancellationToken);
    }

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