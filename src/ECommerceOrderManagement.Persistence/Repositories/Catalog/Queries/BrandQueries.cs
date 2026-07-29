using ECommerceOrderManagement.Application.Features.Catalog.Brands;
using ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrandById;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Catalog.Queries;

internal sealed class BrandQueries(ApplicationDbContext dbContext) : IBrandQueries
{
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