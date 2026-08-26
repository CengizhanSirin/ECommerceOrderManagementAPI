using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Domain.Coupons;

public sealed class CouponUsage : AuditableEntity
{
    public Guid CouponId { get; private set; }

    public Guid UserId { get; private set; }

    public Guid OrderId { get; private set; }

    private CouponUsage()
    {
    }

    private CouponUsage(Guid id, Guid couponId, Guid userId, Guid orderId) : base(id)
    {
        CouponId = couponId;
        UserId = userId;
        OrderId = orderId;
    }

    public static CouponUsage Create(Guid couponId, Guid userId, Guid orderId)
    {
        if (couponId == Guid.Empty)
        {
            throw new ArgumentException("Coupon ID cannot be empty.", nameof(couponId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));
        }

        return new CouponUsage(Guid.NewGuid(), couponId, userId, orderId);
    }
}