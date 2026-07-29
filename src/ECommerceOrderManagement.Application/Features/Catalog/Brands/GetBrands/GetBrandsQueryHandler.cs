using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrands;

public sealed class GetBrandsQueryHandler(IBrandQueries brandQueries) : IQueryHandler<GetBrandsQuery, IReadOnlyCollection<GetBrandsItemResponse>>
{
    public async Task<Result<IReadOnlyCollection<GetBrandsItemResponse>>> Handle(GetBrandsQuery query, CancellationToken cancellationToken)
    {
        var brands = await brandQueries.GetAllAsync(query, cancellationToken);

        return Result<IReadOnlyCollection<GetBrandsItemResponse>>.Success(brands);

    }
}