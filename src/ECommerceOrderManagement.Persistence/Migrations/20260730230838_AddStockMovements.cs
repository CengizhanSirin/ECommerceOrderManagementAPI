using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceOrderManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStockMovements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockMovements",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    QuantityOnHandBefore = table.Column<int>(type: "int", nullable: false),
                    QuantityOnHandAfter = table.Column<int>(type: "int", nullable: false),
                    ReservedQuantityBefore = table.Column<int>(type: "int", nullable: false),
                    ReservedQuantityAfter = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovements", x => x.Id);
                    table.CheckConstraint("CK_StockMovements_Quantity_Positive", "[Quantity] > 0");
                    table.CheckConstraint("CK_StockMovements_QuantityOnHandAfter_NonNegative", "[QuantityOnHandAfter] >= 0");
                    table.CheckConstraint("CK_StockMovements_QuantityOnHandBefore_NonNegative", "[QuantityOnHandBefore] >= 0");
                    table.CheckConstraint("CK_StockMovements_ReservedAfter_NotExceedOnHand", "[ReservedQuantityAfter] <= [QuantityOnHandAfter]");
                    table.CheckConstraint("CK_StockMovements_ReservedBefore_NotExceedOnHand", "[ReservedQuantityBefore] <= [QuantityOnHandBefore]");
                    table.CheckConstraint("CK_StockMovements_ReservedQuantityAfter_NonNegative", "[ReservedQuantityAfter] >= 0");
                    table.CheckConstraint("CK_StockMovements_ReservedQuantityBefore_NonNegative", "[ReservedQuantityBefore] >= 0");
                    table.ForeignKey(
                        name: "FK_StockMovements_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "inventory",
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_InventoryItemId_CreatedAtUtc",
                schema: "inventory",
                table: "StockMovements",
                columns: new[] { "InventoryItemId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ProductId",
                schema: "inventory",
                table: "StockMovements",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockMovements",
                schema: "inventory");
        }
    }
}
