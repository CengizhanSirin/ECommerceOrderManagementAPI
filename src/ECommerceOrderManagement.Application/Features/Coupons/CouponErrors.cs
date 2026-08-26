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

    public static Error DiscountWouldMakeOrderFree()
    {
        return Error.Conflict("Coupon.DiscountWouldMakeOrderFree", "Coupon discount must leave the order total greater than zero.");
    }

    public static Error NotFoundByCode(string code)
    {
        return Error.NotFound("Coupon.NotFound", $"Coupon with code '{code}' was not found.");
    }

    public static Error Inactive(string code)
    {
        return Error.Conflict("Coupon.Inactive", $"Coupon '{code}' is not active.");
    }

    public static Error NotStarted(string code)
    {
        return Error.Conflict("Coupon.NotStarted", $"Coupon '{code}' is not valid yet.");
    }

    public static Error Expired(string code)
    {
        return Error.Conflict("Coupon.Expired", $"Coupon '{code}' has expired.");
    }

    public static Error MinimumOrderAmountNotMet(string code, decimal minimumOrderAmount)
    {
        return Error.Conflict("Coupon.MinimumOrderAmountNotMet",
            $"Coupon '{code}' requires a minimum order amount of {minimumOrderAmount:F2}.");
    }

    public static Error UsageLimitReached(string code)
    {
        return Error.Conflict("Coupon.UsageLimitReached", $"Coupon '{code}' has reached its total usage limit.");
    }

    public static Error UserUsageLimitReached(string code)
    {
        return Error.Conflict("Coupon.UserUsageLimitReached", $"You have reached the usage limit for coupon '{code}'.");
    }

    public static Error DiscountExceedsSubtotal()
    {
        return Error.Conflict("Coupon.DiscountExceedsSubtotal", "Fixed discount amount cannot exceed the order subtotal.");
    }
}