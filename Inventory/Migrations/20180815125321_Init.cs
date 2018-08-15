using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    WarehouseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Main" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ExpiryDate", "Name", "Type", "WarehouseId" },
                values: new object[,]
                {
                    { 1, new DateTime(2018, 8, 16, 12, 53, 21, 773, DateTimeKind.Utc), "Apple", "Food", 1 },
                    { 2, new DateTime(2018, 8, 17, 12, 53, 21, 773, DateTimeKind.Utc), "Banana", "Food", 1 },
                    { 3, new DateTime(2018, 8, 15, 12, 53, 21, 773, DateTimeKind.Utc), "Milk", "Food", 1 },
                    { 4, new DateTime(2018, 8, 15, 12, 53, 21, 773, DateTimeKind.Utc), "Meat", "Food", 1 },
                    { 5, new DateTime(2018, 8, 15, 12, 53, 21, 773, DateTimeKind.Utc), "Bread", "Food", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_WarehouseId",
                table: "Products",
                column: "WarehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Warehouses");
        }
    }
}
