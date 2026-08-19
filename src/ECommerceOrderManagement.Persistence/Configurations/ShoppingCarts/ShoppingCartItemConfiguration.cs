using ECommerceOrderManagement.Domain.ShoppingCarts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.ShoppingCarts;

internal sealed class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder.ToTable("ShoppingCartItems", "shopping",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_ShoppingCartItems_Quantity_Positive", "[Quantity] > 0");
            });

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .ValueGeneratedNever();

        builder.Property(item => item.ShoppingCartId)
            .IsRequired();

        builder.Property(item => item.ProductId)
            .IsRequired();

        builder.Property(item => item.Quantity)
            .IsRequired();

        builder.Property(item => item.CreatedAtUtc)
            .IsRequired();

        builder.Property(item => item.UpdatedAtUtc);

        builder.HasIndex(item => new
        {
            item.ShoppingCartId,
            item.ProductId
        })
        .IsUnique();
    }
}