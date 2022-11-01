using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Infrastructure.Migrations
{
    public partial class Add_StockInPaper_Aggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockInPaper",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    PaperSerialNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StockhouseId = table.Column<long>(type: "bigint", nullable: false),
                    CargoOwnerId = table.Column<long>(type: "bigint", nullable: false),
                    CargoOwnerName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInPaper", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "StockInDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockInPaperId = table.Column<long>(type: "bigint", nullable: false),
                    PaperSerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CargoOwner = table.Column<long>(type: "bigint", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClaimCount = table.Column<int>(type: "int", nullable: false),
                    FactCount = table.Column<int>(type: "int", nullable: false),
                    StorehouseShelfedCount = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInDetail", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_StockInDetail_StockInPaper_StockInPaperId",
                        column: x => x.StockInPaperId,
                        principalTable: "StockInPaper",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockInDetail_CargoOwner",
                table: "StockInDetail",
                column: "CargoOwner");

            migrationBuilder.CreateIndex(
                name: "IX_StockInDetail_PaperSerialNumber",
                table: "StockInDetail",
                column: "PaperSerialNumber")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_StockInDetail_SKU",
                table: "StockInDetail",
                column: "SKU");

            migrationBuilder.CreateIndex(
                name: "IX_StockInDetail_StockInPaperId",
                table: "StockInDetail",
                column: "StockInPaperId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInPaper_PaperSerialNumber",
                table: "StockInPaper",
                column: "PaperSerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_StockInPaper_TenantId_StockhouseId",
                table: "StockInPaper",
                columns: new[] { "TenantId", "StockhouseId" })
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockInDetail");

            migrationBuilder.DropTable(
                name: "StockInPaper");
        }
    }
}
