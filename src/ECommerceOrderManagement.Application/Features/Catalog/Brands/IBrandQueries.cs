using ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrandById;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands;

public interface IBrandQueries
{
    Task<GetBrandByIdResponse?> GetByIdAsync(Guid brandId, CancellationToken cancellationToken = default);
}