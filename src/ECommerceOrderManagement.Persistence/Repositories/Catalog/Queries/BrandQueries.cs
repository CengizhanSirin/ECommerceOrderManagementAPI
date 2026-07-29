using ECommerceOrderManagement.Application.Features.Catalog.Brands;
using ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrandById;
using ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrands;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Catalog.Queries;

internal sealed class BrandQueries(ApplicationDbContext dbContext) : IBrandQueries
{
    public async Task<IReadOnlyCollection<GetBrandsItemResponse>> GetAllAsync(GetBrandsQuery query, CancellationToken cancellationToken = default)
    {
        var brandsQuery = dbContext.Brands.AsNoTracking()
         ;

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.Trim();

            brandsQuery = brandsQuery.Where(brand => brand.Name.Contains(searchTerm) || brand.Slug.Contains(searchTerm));
        }

        if (query.IsActive.HasValue)
        {
            brandsQuery = brandsQuery.Where(brand => brand.IsActive == query.IsActive.Value);

        }

        return await brandsQuery
            .OrderBy(brand => brand.Name)
            .ThenBy(brand => brand.Id)
            .Select(brand => new GetBrandsItemResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                Slug = brand.Slug,
                LogoUrl = brand.LogoUrl,
                IsActive = brand.IsActive
            })
            .ToArrayAsync(cancellationToken);
    }

    public Task<GetBrandByIdResponse?> GetByIdAsync(Guid brandId, CancellationToken cancellationToken = default)
    {
        return dbContext.Brands
            .AsNoTracking()
            .Where(brand => brand.Id == brandId)
            .Select(brand => new GetBrandByIdResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                Slug = brand.Slug,
                Description = brand.Description,
                LogoUrl = brand.LogoUrl,
                IsActive = brand.IsActive,
                CreatedAtUtc = brand.CreatedAtUtc,
                UpdatedAtUtc = brand.UpdatedAtUtc
            })
            .SingleOrDefaultAsync(cancellationToken);
    }
}