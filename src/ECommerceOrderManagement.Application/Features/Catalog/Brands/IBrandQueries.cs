using ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrandById;
using ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrands;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands;

public interface IBrandQueries
{
    Task<GetBrandByIdResponse?> GetByIdAsync(Guid brandId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<GetBrandsItemResponse>> GetAllAsync(GetBrandsQuery query, CancellationToken cancellationToken = default);
}