using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Coupons.DeactivateCoupon;

public sealed record DeactivateCouponCommand(Guid CouponId): ICommand;