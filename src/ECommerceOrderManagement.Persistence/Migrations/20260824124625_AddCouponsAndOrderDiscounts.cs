using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceOrderManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCouponsAndOrderDiscounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "discount");

            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                schema: "orders",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                schema: "orders",
                table: "Orders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                schema: "orders",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountValue",
                schema: "orders",
                table: "Orders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                schema: "orders",
                table: "Orders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                schema: "orders",
                table: "Orders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(
           """
                  UPDATE o
                          SET
                              o.Subtotal = COALESCE(t.Subtotal, 0),
                              o.TotalAmount = COALESCE(t.Subtotal, 0)
                         FROM [orders].[Orders] AS o
                         OUTER APPLY
                    (
                       SELECT SUM(oi.UnitPrice * oi.Quantity) AS Subtotal
                       FROM [orders].[OrderItems] AS oi
                       WHERE oi.OrderId = o.Id
                    ) AS t;
           """);

            migrationBuilder.CreateTable(
                name: "Coupons",
                schema: "discount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MinimumOrderAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsageLimit = table.Column<int>(type: "int", nullable: false),
                    UsageLimitPerUser = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                    table.CheckConstraint("CK_Coupons_DateRange_Valid", "[StartsAtUtc] < [EndsAtUtc]");
                    table.CheckConstraint("CK_Coupons_DiscountValue_Positive", "[DiscountValue] > 0");
                    table.CheckConstraint("CK_Coupons_MinimumOrderAmount_NonNegative", "[MinimumOrderAmount] >= 0");
                    table.CheckConstraint("CK_Coupons_UsageLimit_Positive", "[UsageLimit] > 0");
                    table.CheckConstraint("CK_Coupons_UsageLimitPerUser_NotGreaterThanUsageLimit", "[UsageLimitPerUser] <= [UsageLimit]");
                    table.CheckConstraint("CK_Coupons_UsageLimitPerUser_Positive", "[UsageLimitPerUser] > 0");
                });

            migrationBuilder.CreateTable(
                name: "CouponUsages",
                schema: "discount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CouponId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponUsages_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalSchema: "discount",
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CouponUsages_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Orders_DiscountAmount_NonNegative",
                schema: "orders",
                table: "Orders",
                sql: "[DiscountAmount] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Orders_DiscountAmount_NotGreaterThanSubtotal",
                schema: "orders",
                table: "Orders",
                sql: "[DiscountAmount] <= [Subtotal]");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Orders_Subtotal_NonNegative",
                schema: "orders",
                table: "Orders",
                sql: "[Subtotal] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Orders_TotalAmount_NonNegative",
                schema: "orders",
                table: "Orders",
                sql: "[TotalAmount] >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_Code",
                schema: "discount",
                table: "Coupons",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CouponUsages_CouponId",
                schema: "discount",
                table: "CouponUsages",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponUsages_CouponId_UserId",
                schema: "discount",
                table: "CouponUsages",
                columns: new[] { "CouponId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_CouponUsages_OrderId",
                schema: "discount",
                table: "CouponUsages",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouponUsages",
                schema: "discount");

            migrationBuilder.DropTable(
                name: "Coupons",
                schema: "discount");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Orders_DiscountAmount_NonNegative",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Orders_DiscountAmount_NotGreaterThanSubtotal",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Orders_Subtotal_NonNegative",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Orders_TotalAmount_NonNegative",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CouponCode",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountValue",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                schema: "orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                schema: "orders",
                table: "Orders");
        }
    }
}
