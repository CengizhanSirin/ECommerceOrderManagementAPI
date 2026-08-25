using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Coupons.GetCouponById;

public sealed record GetCouponByIdQuery(Guid CouponId) : IQuery<GetCouponByIdResponse>;