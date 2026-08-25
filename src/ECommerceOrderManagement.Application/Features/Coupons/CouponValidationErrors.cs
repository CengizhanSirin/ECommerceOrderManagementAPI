namespace ECommerceOrderManagement.Application.Features.Coupons;

public static class CouponValidationErrors
{
    public const string CodeRequiredCode = "Coupon.Code.Required";
    public const string CodeRequiredMessage = "Coupon code is required.";

    public const string CodeMaxLengthCode = "Coupon.Code.MaxLength";
    public const string CodeMaxLengthMessage = "Coupon code must not exceed 50 characters.";

    public const string DiscountTypeInvalidCode = "Coupon.DiscountType.Invalid";
    public const string DiscountTypeInvalidMessage = "Discount type is invalid.";

    public const string DiscountValuePositiveCode = "Coupon.DiscountValue.Positive";
    public const string DiscountValuePositiveMessage = "Discount value must be greater than zero.";

    public const string PercentageValueRangeCode = "Coupon.DiscountValue.PercentageRange";
    public const string PercentageValueRangeMessage = "Percentage discount cannot exceed 100.";

    public const string MinimumOrderAmountNonNegativeCode = "Coupon.MinimumOrderAmount.NonNegative";
    public const string MinimumOrderAmountNonNegativeMessage = "Minimum order amount cannot be negative.";

    public const string StartsAtRequiredCode = "Coupon.StartsAtUtc.Required";
    public const string StartsAtRequiredMessage = "Coupon start date is required.";

    public const string EndsAtRequiredCode = "Coupon.EndsAtUtc.Required";
    public const string EndsAtRequiredMessage = "Coupon end date is required.";

    public const string DateRangeInvalidCode = "Coupon.DateRange.Invalid";
    public const string DateRangeInvalidMessage = "Coupon end date must be later than start date.";

    public const string UsageLimitPositiveCode = "Coupon.UsageLimit.Positive";
    public const string UsageLimitPositiveMessage = "Usage limit must be greater than zero.";

    public const string UsageLimitPerUserPositiveCode = "Coupon.UsageLimitPerUser.Positive";
    public const string UsageLimitPerUserPositiveMessage = "Usage limit per user must be greater than zero.";

    public const string UsageLimitPerUserExceededCode = "Coupon.UsageLimitPerUser.Exceeded";
    public const string UsageLimitPerUserExceededMessage = "Usage limit per user cannot exceed total usage limit.";
}