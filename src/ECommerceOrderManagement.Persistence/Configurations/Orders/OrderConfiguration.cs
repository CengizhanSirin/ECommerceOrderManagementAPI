using ECommerceOrderManagement.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Orders;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", "orders",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "CK_Orders_Status_Valid",
                    "[Status] BETWEEN 1 AND 6");

                tableBuilder.HasCheckConstraint(
                    "CK_Orders_Subtotal_NonNegative",
                    "[Subtotal] >= 0");

                tableBuilder.HasCheckConstraint(
                    "CK_Orders_DiscountAmount_NonNegative",
                    "[DiscountAmount] >= 0");

                tableBuilder.HasCheckConstraint(
                    "CK_Orders_TotalAmount_NonNegative",
                    "[TotalAmount] >= 0");

                tableBuilder.HasCheckConstraint(
                    "CK_Orders_DiscountAmount_NotGreaterThanSubtotal",
                    "[DiscountAmount] <= [Subtotal]");
            });


        builder.HasKey(order => order.Id);

        builder.Property(order => order.Id).ValueGeneratedNever();

        builder.Property(order => order.CustomerId).IsRequired();

        builder.Property(order => order.OrderNumber).HasMaxLength(50).IsRequired();

        builder.Property(order => order.Status).HasConversion<int>().IsRequired();

        builder.Property(order => order.CancellationReason).HasMaxLength(500);

        builder.Property(order => order.Subtotal).HasPrecision(18, 2).IsRequired();

        builder.Property(order => order.DiscountAmount).HasPrecision(18, 2).IsRequired();

        builder.Property(order => order.TotalAmount).HasPrecision(18, 2).IsRequired();

        builder.Property(order => order.CouponCode).HasMaxLength(50);

        builder.Property(order => order.DiscountType);

        builder.Property(order => order.DiscountValue).HasPrecision(18, 2);

        ConfigureShippingAddress(builder);

        ConfigureBillingAddress(builder);

        builder.HasMany(order => order.Items)
            .WithOne()
            .HasForeignKey(orderItem => orderItem.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(order => order.Items).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(order => order.OrderNumber).IsUnique();

        builder.HasIndex(order => order.CustomerId);

        builder.HasIndex(order => order.Status);

        builder.HasIndex(order => order.CreatedAtUtc);

        builder.Property<byte[]>("RowVersion").IsRequired().IsRowVersion();
    }

    private static void ConfigureShippingAddress(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(order => order.ShippingAddress,
            addressBuilder =>
            {
                addressBuilder.Property(address => address.FullName)
                    .HasColumnName("ShippingFullName")
                    .HasMaxLength(150)
                    .IsRequired();

                addressBuilder.Property(address => address.PhoneNumber)
                    .HasColumnName("ShippingPhoneNumber")
                    .HasMaxLength(30)
                    .IsRequired();

                addressBuilder.Property(address => address.Country)
                    .HasColumnName("ShippingCountry")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(address => address.City)
                    .HasColumnName("ShippingCity")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(address => address.District)
                    .HasColumnName("ShippingDistrict")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(address => address.PostalCode)
                    .HasColumnName("ShippingPostalCode")
                    .HasMaxLength(20);

                addressBuilder.Property(address => address.AddressLine)
                    .HasColumnName("ShippingAddressLine")
                    .HasMaxLength(500)
                    .IsRequired();
            });
    }

    private static void ConfigureBillingAddress(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(order => order.BillingAddress,
            addressBuilder =>
            {
                addressBuilder.Property(address => address.FullName)
                    .HasColumnName("BillingFullName")
                    .HasMaxLength(150)
                    .IsRequired();

                addressBuilder.Property(address => address.PhoneNumber)
                    .HasColumnName("BillingPhoneNumber")
                    .HasMaxLength(30)
                    .IsRequired();

                addressBuilder.Property(address => address.Country)
                    .HasColumnName("BillingCountry")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(address => address.City)
                    .HasColumnName("BillingCity")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(address => address.District)
                    .HasColumnName("BillingDistrict")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(address => address.PostalCode)
                    .HasColumnName("BillingPostalCode")
                    .HasMaxLength(20);

                addressBuilder.Property(address => address.AddressLine)
                    .HasColumnName("BillingAddressLine")
                    .HasMaxLength(500)
                    .IsRequired();
            });
    }
}