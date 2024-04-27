using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothings_Store.Migrations
{
    /// <inheritdoc />
    public partial class FlutterDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderFlutters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlutterAccountId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderFlutters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetailFlutters",
                columns: table => new
                {
                    OrderFlutterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDetailFlutter", x => new { x.OrderFlutterId, x.StockId });
                    table.ForeignKey(
                        name: "FK_OrderDetailFlutters_OrderFlutters_OrderFlutterId",
                        column: x => x.OrderFlutterId,
                        principalTable: "OrderFlutters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetailFlutters_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetailFlutters_StockId",
                table: "OrderDetailFlutters",
                column: "StockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetailFlutters");

            migrationBuilder.DropTable(
                name: "OrderFlutters");
        }
    }
}
