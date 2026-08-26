using ECommerceOrderManagement.Domain.Coupons;

namespace ECommerceOrderManagement.Application.Features.Coupons.GetCoupons;

public sealed record GetCouponsItemResponse(
    Guid Id,
    string Code,
    DiscountType DiscountType,
    decimal DiscountValue,
    decimal MinimumOrderAmount,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    int UsageLimit,
    int UsageLimitPerUser,
    int TotalUsageCount,
    bool IsActive,
    DateTime CreatedAtUtc);