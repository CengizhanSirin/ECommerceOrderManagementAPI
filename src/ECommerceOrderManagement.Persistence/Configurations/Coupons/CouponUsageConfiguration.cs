using ECommerceOrderManagement.Domain.Coupons;
using ECommerceOrderManagement.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Coupons;

internal sealed class CouponUsageConfiguration : IEntityTypeConfiguration<CouponUsage>
{
    public void Configure(EntityTypeBuilder<CouponUsage> builder)
    {
        builder.ToTable("CouponUsages", "discount");

        builder.HasKey(usage => usage.Id);

        builder.Property(usage => usage.Id)
            .ValueGeneratedNever();

        builder.Property(usage => usage.CouponId)
            .IsRequired();

        builder.Property(usage => usage.UserId)
            .IsRequired();

        builder.Property(usage => usage.OrderId)
            .IsRequired();

        builder.Property(usage => usage.CreatedAtUtc)
            .IsRequired();

        builder.Property(usage => usage.UpdatedAtUtc);

        builder.HasIndex(usage => usage.CouponId);

        builder.HasIndex(usage => new
        {
            usage.CouponId,
            usage.UserId
        });

        builder.HasIndex(usage => usage.OrderId)
            .IsUnique();

        builder.HasOne<Coupon>()
            .WithMany()
            .HasForeignKey(usage => usage.CouponId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Order>()
            .WithMany()
            .HasForeignKey(usage => usage.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}