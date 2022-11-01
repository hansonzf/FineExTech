using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Merchant.Api.Infrastructure.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partner",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MerchantCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactBook = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "[]"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partner", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partner_TenantId",
                table: "Partner",
                column: "TenantId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_Partner_TenantId_MerchantCode",
                table: "Partner",
                columns: new[] { "TenantId", "MerchantCode" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partner");
        }
    }
}
