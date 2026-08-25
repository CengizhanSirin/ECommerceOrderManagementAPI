using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Domain.Coupons;

namespace ECommerceOrderManagement.Application.Features.Coupons.CreateCoupon;

public sealed record CreateCouponCommand(string Code, DiscountType DiscountType, decimal DiscountValue, decimal MinimumOrderAmount, DateTime StartsAtUtc,
    DateTime EndsAtUtc, int UsageLimit, int UsageLimitPerUser)
    : ICommand<CreateCouponResponse>;