using ECommerceOrderManagement.Application.Features.Catalog.Products.GetProductById;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products;

public interface IProductQueries
{
    Task<GetProductByIdResponse?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);
}