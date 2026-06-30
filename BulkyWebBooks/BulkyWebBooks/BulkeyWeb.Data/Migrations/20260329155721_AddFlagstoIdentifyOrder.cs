using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkeyWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFlagstoIdentifyOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ShoppingCarts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsOrderPlaced",
                table: "ShoppingCarts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ShoppingCarts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "IsOrderPlaced",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ShoppingCarts");
        }
    }
}
