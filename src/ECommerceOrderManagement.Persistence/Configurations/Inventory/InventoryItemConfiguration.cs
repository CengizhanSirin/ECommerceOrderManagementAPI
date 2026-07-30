using ECommerceOrderManagement.Domain.Catalog;
using ECommerceOrderManagement.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Inventory;

internal sealed class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("InventoryItems", "inventory",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "CK_InventoryItems_QuantityOnHand_NonNegative",
                    "[QuantityOnHand] >= 0");

                tableBuilder.HasCheckConstraint(
                    "CK_InventoryItems_ReservedQuantity_NonNegative",
                    "[ReservedQuantity] >= 0");

                tableBuilder.HasCheckConstraint(
                    "CK_InventoryItems_ReservedQuantity_NotGreaterThanQuantityOnHand",
                    "[ReservedQuantity] <= [QuantityOnHand]");

                tableBuilder.HasCheckConstraint(
                    "CK_InventoryItems_ReorderLevel_NonNegative",
                    "[ReorderLevel] >= 0");
            });


        builder.HasKey(inventoryItem => inventoryItem.Id);

        builder.Property(inventoryItem => inventoryItem.ProductId).IsRequired();
        builder.Property(inventoryItem => inventoryItem.QuantityOnHand).IsRequired();
        builder.Property(inventoryItem => inventoryItem.ReservedQuantity).IsRequired();
        builder.Property(inventoryItem => inventoryItem.ReorderLevel).IsRequired();
        builder.HasIndex(inventoryItem => inventoryItem.ProductId).IsUnique();

        builder.HasOne<Product>()
            .WithOne()
            .HasForeignKey<InventoryItem>(inventoryItem => inventoryItem.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<byte[]>("RowVersion").IsRowVersion();


        builder.Ignore(inventoryItem => inventoryItem.AvailableQuantity);

        builder.Ignore(inventoryItem => inventoryItem.IsLowStock);

        builder.Ignore(inventoryItem => inventoryItem.IsOutOfStock);
    }
}