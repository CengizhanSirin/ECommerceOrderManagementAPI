using ECommerceOrderManagement.Domain.Coupons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Coupons;

internal sealed class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons", "discount",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "CK_Coupons_DiscountValue_Positive",
                    "[DiscountValue] > 0");

                tableBuilder.HasCheckConstraint(
                    "CK_Coupons_MinimumOrderAmount_NonNegative",
                    "[MinimumOrderAmount] >= 0");

                tableBuilder.HasCheckConstraint(
                    "CK_Coupons_UsageLimit_Positive",
                    "[UsageLimit] > 0");

                tableBuilder.HasCheckConstraint(
                    "CK_Coupons_UsageLimitPerUser_Positive",
                    "[UsageLimitPerUser] > 0");

                tableBuilder.HasCheckConstraint(
                    "CK_Coupons_UsageLimitPerUser_NotGreaterThanUsageLimit",
                    "[UsageLimitPerUser] <= [UsageLimit]");

                tableBuilder.HasCheckConstraint(
                    "CK_Coupons_DateRange_Valid",
                    "[StartsAtUtc] < [EndsAtUtc]");
            });

        builder.HasKey(coupon => coupon.Id);

        builder.Property(coupon => coupon.Id)
            .ValueGeneratedNever();

        builder.Property(coupon => coupon.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(coupon => coupon.DiscountType)
            .IsRequired();

        builder.Property(coupon => coupon.DiscountValue)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(coupon => coupon.MinimumOrderAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(coupon => coupon.StartsAtUtc)
            .IsRequired();

        builder.Property(coupon => coupon.EndsAtUtc)
            .IsRequired();

        builder.Property(coupon => coupon.UsageLimit)
            .IsRequired();

        builder.Property(coupon => coupon.UsageLimitPerUser)
            .IsRequired();

        builder.Property(coupon => coupon.IsActive)
            .IsRequired();

        builder.Property(coupon => coupon.CreatedAtUtc)
            .IsRequired();

        builder.Property(coupon => coupon.UpdatedAtUtc);

        builder.HasIndex(coupon => coupon.Code)
            .IsUnique();

        builder.Ignore(coupon => coupon.DomainEvents);
    }
}