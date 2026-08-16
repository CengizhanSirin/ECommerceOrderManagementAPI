using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceOrderManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorAddressDefaultsToPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAddressPreferences",
                schema: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(
                        type: "uniqueidentifier",
                        nullable: false),

                    UserId = table.Column<Guid>(
                        type: "uniqueidentifier",
                        nullable: false),

                    DefaultShippingAddressId = table.Column<Guid>(
                        type: "uniqueidentifier",
                        nullable: true),

                    DefaultBillingAddressId = table.Column<Guid>(
                        type: "uniqueidentifier",
                        nullable: true),

                    CreatedAtUtc = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false),

                    UpdatedAtUtc = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_UserAddressPreferences",
                        x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAddressPreferences_UserId",
                schema: "users",
                table: "UserAddressPreferences",
                column: "UserId",
                unique: true);

            migrationBuilder.Sql(
                """
        INSERT INTO [users].[UserAddressPreferences]
        (
            [Id],
            [UserId],
            [DefaultShippingAddressId],
            [DefaultBillingAddressId],
            [CreatedAtUtc],
            [UpdatedAtUtc]
        )
        SELECT
            NEWID(),
            [UserId],
            [Id],
            [Id],
            SYSUTCDATETIME(),
            NULL
        FROM [users].[Addresses]
        WHERE [IsDefault] = 1
          AND [IsDeleted] = 0;
        """);

            migrationBuilder.DropIndex(
                name: "UX_Addresses_UserId_Default_NotDeleted",
                schema: "users",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                schema: "users",
                table: "Addresses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAddressPreferences",
                schema: "users");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                schema: "users",
                table: "Addresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "UX_Addresses_UserId_Default_NotDeleted",
                schema: "users",
                table: "Addresses",
                column: "UserId",
                unique: true,
                filter: "[IsDefault] = 1 AND [IsDeleted] = 0");
        }
    }
}
