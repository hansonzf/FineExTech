using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Infrastructure.Migrations
{
    public partial class Modify_Storehouse_Aggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_StockLocation_Storehouses_StorehouseId",
                table: "StockLocation",
                column: "StorehouseId",
                principalTable: "Storehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockLocation_Storehouses_StorehouseId",
                table: "StockLocation");
        }
    }
}
