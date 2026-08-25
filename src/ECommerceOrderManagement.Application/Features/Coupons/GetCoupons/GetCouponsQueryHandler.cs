using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Coupons.GetCoupons;

internal sealed class GetCouponsQueryHandler(ICouponQueries couponQueries) : IQueryHandler<GetCouponsQuery, PagedResult<GetCouponsItemResponse>>
{
    public async Task<Result<PagedResult<GetCouponsItemResponse>>> Handle(GetCouponsQuery query, CancellationToken cancellationToken)
    {
        var coupons = await couponQueries.GetPagedAsync(query, cancellationToken);

        return Result<PagedResult<GetCouponsItemResponse>>.Success(coupons);
    }
}