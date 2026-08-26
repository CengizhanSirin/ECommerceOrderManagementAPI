using ECommerceOrderManagement.Domain.Coupons;

namespace ECommerceOrderManagement.API.Features.Coupons.CreateCoupon;

public sealed record CreateCouponRequest(string Code, DiscountType DiscountType, decimal DiscountValue, decimal MinimumOrderAmount, DateTime StartsAtUtc, DateTime EndsAtUtc,
    int UsageLimit, int UsageLimitPerUser);