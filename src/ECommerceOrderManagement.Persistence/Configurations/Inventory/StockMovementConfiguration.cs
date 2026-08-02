using ECommerceOrderManagement.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Inventory;

internal sealed class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements", "inventory",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_StockMovements_Quantity_Positive",
                    "[Quantity] > 0");

                tableBuilder.HasCheckConstraint("CK_StockMovements_QuantityOnHandBefore_NonNegative",
                    "[QuantityOnHandBefore] >= 0");

                tableBuilder.HasCheckConstraint("CK_StockMovements_QuantityOnHandAfter_NonNegative",
                    "[QuantityOnHandAfter] >= 0");

                tableBuilder.HasCheckConstraint("CK_StockMovements_ReservedQuantityBefore_NonNegative",
                    "[ReservedQuantityBefore] >= 0");

                tableBuilder.HasCheckConstraint("CK_StockMovements_ReservedQuantityAfter_NonNegative",
                    "[ReservedQuantityAfter] >= 0");

                tableBuilder.HasCheckConstraint("CK_StockMovements_ReservedBefore_NotExceedOnHand",
                    "[ReservedQuantityBefore] <= [QuantityOnHandBefore]");

                tableBuilder.HasCheckConstraint("CK_StockMovements_ReservedAfter_NotExceedOnHand",
                    "[ReservedQuantityAfter] <= [QuantityOnHandAfter]");
            });

        builder.HasKey(stockMovement => stockMovement.Id);

        builder.Property(stockMovement => stockMovement.InventoryItemId).IsRequired();

        builder.Property(stockMovement => stockMovement.Id).ValueGeneratedNever();

        builder.Property(stockMovement => stockMovement.ProductId).IsRequired();

        builder.Property(stockMovement => stockMovement.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(stockMovement => stockMovement.Quantity).IsRequired();

        builder.Property(stockMovement => stockMovement.QuantityOnHandBefore).IsRequired();

        builder.Property(stockMovement => stockMovement.QuantityOnHandAfter).IsRequired();

        builder.Property(stockMovement => stockMovement.ReservedQuantityBefore).IsRequired();

        builder.Property(stockMovement => stockMovement.ReservedQuantityAfter).IsRequired();

        builder.Property(stockMovement => stockMovement.Reason).HasMaxLength(500);

        builder.HasIndex(stockMovement => stockMovement.ProductId);

        builder.HasIndex(stockMovement => new
        {
            stockMovement.InventoryItemId,
            stockMovement.CreatedAtUtc
        });
    }
}