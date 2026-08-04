using ECommerceOrderManagement.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Orders;

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", "orders",
              tableBuilder =>
              {
                  tableBuilder.HasCheckConstraint("CK_OrderItems_UnitPrice_NonNegative",
                      "[UnitPrice] >= 0");

                  tableBuilder.HasCheckConstraint("CK_OrderItems_Quantity_Positive",
                      "[Quantity] > 0");
              });

        builder.HasKey(orderItem => orderItem.Id);

        builder.Property(orderItem => orderItem.Id).ValueGeneratedNever();

        builder.Property(orderItem => orderItem.OrderId).IsRequired();

        builder.Property(orderItem => orderItem.ProductId).IsRequired();

        builder.Property(orderItem => orderItem.ProductName).HasMaxLength(200).IsRequired();

        builder.Property(orderItem => orderItem.Sku).HasMaxLength(100).IsRequired();

        builder.Property(orderItem => orderItem.UnitPrice).HasPrecision(18, 2).IsRequired();

        builder.Property(orderItem => orderItem.Quantity).IsRequired();

        builder.Ignore(orderItem => orderItem.LineTotal);

        builder.HasIndex(orderItem => orderItem.ProductId);

        builder.HasIndex(orderItem => new
        {
            orderItem.OrderId,
            orderItem.ProductId
        })
            .IsUnique();
    }
}