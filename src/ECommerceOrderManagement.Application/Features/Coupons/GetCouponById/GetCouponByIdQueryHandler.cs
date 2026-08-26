using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Coupons.GetCouponById;

internal sealed class GetCouponByIdQueryHandler(ICouponQueries couponQueries) : IQueryHandler<GetCouponByIdQuery, GetCouponByIdResponse>
{
    public async Task<Result<GetCouponByIdResponse>> Handle(GetCouponByIdQuery query, CancellationToken cancellationToken)
    {
        var coupon = await couponQueries.GetByIdAsync(query.CouponId, cancellationToken);

        if (coupon is null)
        {
            return Result<GetCouponByIdResponse>.Failure(CouponErrors.NotFound(query.CouponId));
        }

        return Result<GetCouponByIdResponse>.Success(coupon);
    }
}