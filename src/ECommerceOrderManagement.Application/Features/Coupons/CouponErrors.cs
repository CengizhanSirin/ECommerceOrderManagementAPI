using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Coupons;

public static class CouponErrors
{
    public static Error NotFound(Guid couponId)
    {
        return Error.NotFound("Coupon.NotFound", $"Coupon with ID '{couponId}' was not found.");
    }

    public static Error CodeAlreadyExists(string code)
    {
        return Error.Conflict("Coupon.CodeAlreadyExists", $"A coupon with the code '{code}' already exists.");
    }
}