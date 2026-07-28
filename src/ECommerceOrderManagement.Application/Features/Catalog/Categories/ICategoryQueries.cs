using ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategoryById;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories;

public interface ICategoryQueries
{
    Task<GetCategoryByIdResponse?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
}