using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceOrderManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeOrderTotalAmountPositive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Orders_TotalAmount_NonNegative",
                schema: "orders",
                table: "Orders");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Orders_TotalAmount_Positive",
                schema: "orders",
                table: "Orders",
                sql: "[TotalAmount] > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Orders_TotalAmount_Positive",
                schema: "orders",
                table: "Orders");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Orders_TotalAmount_NonNegative",
                schema: "orders",
                table: "Orders",
                sql: "[TotalAmount] >= 0");
        }
    }
}
