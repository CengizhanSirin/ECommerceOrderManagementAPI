using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrandById;

internal class GetBrandByIdQueryHandler(IBrandQueries brandQueries) : IQueryHandler<GetBrandByIdQuery, GetBrandByIdResponse>
{
    public async Task<Result<GetBrandByIdResponse>> Handle(GetBrandByIdQuery query, CancellationToken cancellationToken)
    {
        var brand = await brandQueries.GetByIdAsync(query.BrandId, cancellationToken);

        if (brand is null)
        {
            return Result<GetBrandByIdResponse>.Failure(BrandErrors.NotFound(query.BrandId));
        }

        return Result<GetBrandByIdResponse>.Success(brand);
    }
}