using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Infrastructure.Migrations
{
    public partial class Add_InventoryRecord_Aggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StorehouseId = table.Column<long>(type: "bigint", nullable: false),
                    CargoOwner = table.Column<long>(type: "bigint", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StockCount = table.Column<int>(type: "int", nullable: false),
                    OpenCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryRecord", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryRecord_StorehouseId",
                table: "InventoryRecord",
                column: "StorehouseId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryRecord_StorehouseId_CargoOwner_SKU_LocationCode",
                table: "InventoryRecord",
                columns: new[] { "StorehouseId", "CargoOwner", "SKU", "LocationCode" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryRecord");
        }
    }
}
