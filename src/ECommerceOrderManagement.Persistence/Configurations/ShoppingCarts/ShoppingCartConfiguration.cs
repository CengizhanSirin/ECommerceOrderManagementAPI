using ECommerceOrderManagement.Domain.ShoppingCarts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.ShoppingCarts;

internal class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.ToTable("ShoppingCarts", "shopping");

        builder.HasKey(cart => cart.Id);

        builder.Property(cart => cart.Id)
            .ValueGeneratedNever();

        builder.Property(cart => cart.UserId)
            .IsRequired();

        builder.Property(cart => cart.CreatedAtUtc)
            .IsRequired();

        builder.Property(cart => cart.UpdatedAtUtc);

        builder.HasIndex(cart => cart.UserId)
            .IsUnique();

        builder.HasMany(cart => cart.Items)
            .WithOne()
            .HasForeignKey(item => item.ShoppingCartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(cart => cart.DomainEvents);
    }
}