using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothings_Store.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrderFlutterModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderStatus",
                table: "OrderFlutters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderTime",
                table: "OrderFlutters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "OrderFlutters");

            migrationBuilder.DropColumn(
                name: "OrderTime",
                table: "OrderFlutters");
        }
    }
}
