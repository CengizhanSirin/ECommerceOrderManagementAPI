using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Coupons.ActivateCoupon;

public sealed record ActivateCouponCommand(Guid CouponId): ICommand;