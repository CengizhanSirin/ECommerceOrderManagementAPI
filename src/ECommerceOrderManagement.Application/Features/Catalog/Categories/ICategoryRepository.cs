using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Domain.Catalog;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<bool> ExistsByNameAsync(string name, Guid? excludedCategoryId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsBySlugAsync(string slug, Guid? excludedCategoryId = null, CancellationToken cancellationToken = default);

}