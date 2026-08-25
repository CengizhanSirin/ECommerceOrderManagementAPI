using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Domain.Coupons;

public sealed class Coupon : AggregateRoot
{
    public string Code { get; private set; } = string.Empty;

    public DiscountType DiscountType { get; private set; }

    public decimal DiscountValue { get; private set; }

    public decimal MinimumOrderAmount { get; private set; }

    public DateTime StartsAtUtc { get; private set; }

    public DateTime EndsAtUtc { get; private set; }

    public int UsageLimit { get; private set; }

    public int UsageLimitPerUser { get; private set; }

    public bool IsActive { get; private set; }

    private Coupon()
    {
    }

    private Coupon(Guid id, string code, DiscountType discountType, decimal discountValue, decimal minimumOrderAmount, DateTime startsAtUtc, DateTime endsAtUtc,
        int usageLimit, int usageLimitPerUser)
        : base(id)
    {
        Code = code;
        DiscountType = discountType;
        DiscountValue = discountValue;
        MinimumOrderAmount = minimumOrderAmount;
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;
        UsageLimit = usageLimit;
        UsageLimitPerUser = usageLimitPerUser;
        IsActive = true;
    }

    public static Coupon Create(string code, DiscountType discountType, decimal discountValue, decimal minimumOrderAmount,
        DateTime startsAtUtc, DateTime endsAtUtc, int usageLimit, int usageLimitPerUser)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        if (discountValue <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(discountValue), "Discount value must be greater than zero.");
        }

        if (discountType == DiscountType.Percentage && discountValue > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(discountValue), "Percentage discount cannot exceed 100.");
        }

        if (minimumOrderAmount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumOrderAmount), "Minimum order amount cannot be negative.");
        }

        if (startsAtUtc >= endsAtUtc)
        {
            throw new ArgumentException("Coupon end date must be later than start date.");
        }

        if (usageLimit <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(usageLimit), "Usage limit must be greater than zero.");
        }

        if (usageLimitPerUser <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(usageLimitPerUser), "Usage limit per user must be greater than zero.");
        }

        if (usageLimitPerUser > usageLimit)
        {
            throw new ArgumentException("Usage limit per user cannot exceed total usage limit.");
        }

        return new Coupon(Guid.NewGuid(), code.Trim().ToUpperInvariant(),
            discountType,
            discountValue,
            minimumOrderAmount,
            startsAtUtc,
            endsAtUtc,
            usageLimit,
            usageLimitPerUser);
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}