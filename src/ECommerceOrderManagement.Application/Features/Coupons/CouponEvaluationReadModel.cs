using ECommerceOrderManagement.Domain.Coupons;

namespace ECommerceOrderManagement.Application.Features.Coupons;

public sealed record CouponEvaluationReadModel(
    Guid Id,
    string Code,
    DiscountType DiscountType,
    decimal DiscountValue,
    decimal MinimumOrderAmount,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    int UsageLimit,
    int UsageLimitPerUser,
    bool IsActive,
    int TotalUsageCount,
    int UserUsageCount);