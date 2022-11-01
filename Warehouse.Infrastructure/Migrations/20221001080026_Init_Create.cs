using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Infrastructure.Migrations
{
    public partial class Init_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockLocation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StorehouseId = table.Column<long>(type: "bigint", nullable: false),
                    LocationCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaxVolume = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    UsedVolume = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    Useable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AllowOverload = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLocation", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Storehouses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MerchantId = table.Column<long>(type: "bigint", nullable: false),
                    WarehouseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WarehouseAddress_Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarehouseAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarehouseAddress_Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarehouseAddress_DetailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storehouses", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockLocation_LocationCode_StorehouseId",
                table: "StockLocation",
                columns: new[] { "LocationCode", "StorehouseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockLocation_StorehouseId",
                table: "StockLocation",
                column: "StorehouseId")
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockLocation");

            migrationBuilder.DropTable(
                name: "Storehouses");
        }
    }
}
