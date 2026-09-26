using ECommerceOrderManagement.Domain.Coupons;

namespace ECommerceOrderManagement.UnitTests.Domain.Coupons;

public sealed class CouponTests
{
    [Fact]
    public void Create_ShouldThrowArgumentOutOfRangeException_WhenPercentageExceeds100()
    {
        // Arrange
        var startsAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var endsAtUtc = startsAtUtc.AddDays(30);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Coupon.Create(
                code: "DISCOUNT",
                discountType: DiscountType.Percentage,
                discountValue: 101m,
                minimumOrderAmount: 0m,
                startsAtUtc: startsAtUtc,
                endsAtUtc: endsAtUtc,
                usageLimit: 100,
                usageLimitPerUser: 1));
    }

    [Fact]
    public void Create_ShouldThrowArgumentException_WhenEndDateIsBeforeStartDate()
    {
        // Arrange
        var startsAtUtc = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc);

        var endsAtUtc = startsAtUtc.AddDays(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Coupon.Create(
                code: "DISCOUNT",
                discountType: DiscountType.Percentage,
                discountValue: 10m,
                minimumOrderAmount: 0m,
                startsAtUtc: startsAtUtc,
                endsAtUtc: endsAtUtc,
                usageLimit: 100,
                usageLimitPerUser: 1));
    }
}