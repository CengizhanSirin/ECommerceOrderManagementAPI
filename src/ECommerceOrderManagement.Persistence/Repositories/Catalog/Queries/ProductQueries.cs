using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Application.Features.Catalog.Products.GetProductById;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Catalog.Queries;

internal sealed class ProductQueries(ApplicationDbContext dbContext) : IProductQueries
{
    public Task<GetProductByIdResponse?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return dbContext.Products.AsNoTracking().Where(product => product.Id == productId).Select(product => new GetProductByIdResponse
        {
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            Sku = product.Sku,
            Description = product.Description,
            PriceAmount = product.Price.Amount,
            Currency = product.Price.Currency,
            CategoryId = product.CategoryId,
            CategoryName = dbContext.Categories
                   .Where(category => category.Id == product.CategoryId)
                   .Select(category => category.Name)
                   .Single(),
            BrandId = product.BrandId,
            BrandName = product.BrandId.HasValue ? dbContext.Brands
                   .Where(brand => brand.Id == product.BrandId.Value)
                   .Select(brand => brand.Name)
                   .Single() : null,
            IsActive = product.IsActive,
            MainImageUrl = product.MainImageUrl,
            CreatedAtUtc = product.CreatedAtUtc,
            UpdatedAtUtc = product.UpdatedAtUtc
        })
       .SingleOrDefaultAsync(cancellationToken);
    }
}
