using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Application.Features.Catalog.Products.GetProductById;
using ECommerceOrderManagement.Application.Features.Catalog.Products.GetProducts;
using ECommerceOrderManagement.Domain.Catalog;
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

    public async Task<PagedResult<GetProductsItemResponse>> GetPagedAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Product> productsQuery = dbContext.Products.AsNoTracking();


        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.Trim();

            productsQuery = productsQuery.Where(product =>
                product.Name.Contains(searchTerm) ||
                product.Slug.Contains(searchTerm) ||
                product.Sku.Contains(searchTerm));
        }

        if (query.CategoryId.HasValue)
        {
            productsQuery = productsQuery.Where(product => product.CategoryId == query.CategoryId.Value);
               
        }

        if (query.BrandId.HasValue)
        {
            productsQuery = productsQuery.Where(product =>product.BrandId == query.BrandId.Value);
                
        }

        if (query.IsActive.HasValue)
        {
            productsQuery = productsQuery.Where(product => product.IsActive == query.IsActive.Value);
               
        }

        var totalCount = await productsQuery.CountAsync(cancellationToken);
            

        productsQuery = ApplySorting(productsQuery,query.SortBy, query.SortDirection);

        var items = await productsQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(product => new GetProductsItemResponse
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Sku = product.Sku,
                PriceAmount = product.Price.Amount,
                Currency = product.Price.Currency,
                CategoryId = product.CategoryId,
                CategoryName = dbContext.Categories
                    .Where(category =>category.Id == product.CategoryId)
                    .Select(category => category.Name)
                    .Single(),
                BrandId = product.BrandId,
                BrandName = product.BrandId.HasValue
                    ? dbContext.Brands
                        .Where(brand =>brand.Id == product.BrandId.Value) 
                        .Select(brand => brand.Name)
                        .Single()
                    : null,
                IsActive = product.IsActive,
                MainImageUrl = product.MainImageUrl,
                CreatedAtUtc = product.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<GetProductsItemResponse>(items,query.PageNumber, query.PageSize, totalCount);
    }

    private static IQueryable<Product> ApplySorting(IQueryable<Product> productsQuery, ProductSortField sortBy, SortDirection sortDirection)
    {
        if (sortDirection == SortDirection.Ascending)
        {
            return sortBy switch
            {
                ProductSortField.Name =>
                         productsQuery
                        .OrderBy(product => product.Name)
                        .ThenBy(product => product.Id),

                ProductSortField.Price =>
                         productsQuery
                        .OrderBy(product => product.Price.Amount)
                        .ThenBy(product => product.Id),

                _ =>
                         productsQuery
                        .OrderBy(product => product.CreatedAtUtc)
                        .ThenBy(product => product.Id)
            };
        }

        return sortBy switch
        {
            ProductSortField.Name =>
                     productsQuery
                    .OrderByDescending(product => product.Name)
                    .ThenBy(product => product.Id),

            ProductSortField.Price =>
                     productsQuery
                    .OrderByDescending(product => product.Price.Amount)
                    .ThenBy(product => product.Id),

            _ =>
                    productsQuery
                    .OrderByDescending(product => product.CreatedAtUtc)
                    .ThenBy(product => product.Id)
        };
    }
}
