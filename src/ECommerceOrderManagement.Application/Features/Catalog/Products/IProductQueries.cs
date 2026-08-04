using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Catalog.Products.GetProductById;
using ECommerceOrderManagement.Application.Features.Catalog.Products.GetProducts;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products;

public interface IProductQueries
{
    Task<GetProductByIdResponse?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task<PagedResult<GetProductsItemResponse>> GetPagedAsync(GetProductsQuery query, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ProductOrderSnapshotDto>> GetActiveOrderSnapshotsByIdsAsync(IReadOnlyCollection<Guid> productIds, CancellationToken cancellationToken = default);
}