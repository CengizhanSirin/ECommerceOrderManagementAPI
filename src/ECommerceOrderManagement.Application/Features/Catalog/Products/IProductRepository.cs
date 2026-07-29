using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Domain.Catalog;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products;

public interface IProductRepository : IRepository<Product>
{
    Task<bool> ExistsBySkuAsync(string sku, Guid? excludedProductId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsBySlugAsync(string slug, Guid? excludedProductId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
}