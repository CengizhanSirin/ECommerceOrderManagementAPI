using ECommerceOrderManagement.Domain.Coupons;

namespace ECommerceOrderManagement.Application.Features.Coupons.GetCouponById;

public sealed record GetCouponByIdResponse(Guid Id,
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
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);