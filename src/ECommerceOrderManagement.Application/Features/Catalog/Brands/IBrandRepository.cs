using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Domain.Catalog;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands;

public interface IBrandRepository : IRepository<Brand>
{
    Task<bool> ExistsByNameAsync(string name, Guid? excludedBrandId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsBySlugAsync(string slug, Guid? excludedBrandId = null, CancellationToken cancellationToken = default);
}