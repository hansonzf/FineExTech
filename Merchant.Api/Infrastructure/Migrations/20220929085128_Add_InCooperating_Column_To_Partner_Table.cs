using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Merchant.Api.Infrastructure.Migrations
{
    public partial class Add_InCooperating_Column_To_Partner_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Partner",
                table: "Partner");

            migrationBuilder.RenameTable(
                name: "Partner",
                newName: "Partners");

            migrationBuilder.RenameIndex(
                name: "IX_Partner_TenantId_MerchantCode",
                table: "Partners",
                newName: "IX_Partners_TenantId_MerchantCode");

            migrationBuilder.RenameIndex(
                name: "IX_Partner_TenantId",
                table: "Partners",
                newName: "IX_Partners_TenantId");

            migrationBuilder.AddColumn<bool>(
                name: "InCooperating",
                table: "Partners",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Partners",
                table: "Partners",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Partners",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "InCooperating",
                table: "Partners");

            migrationBuilder.RenameTable(
                name: "Partners",
                newName: "Partner");

            migrationBuilder.RenameIndex(
                name: "IX_Partners_TenantId_MerchantCode",
                table: "Partner",
                newName: "IX_Partner_TenantId_MerchantCode");

            migrationBuilder.RenameIndex(
                name: "IX_Partners_TenantId",
                table: "Partner",
                newName: "IX_Partner_TenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Partner",
                table: "Partner",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);
        }
    }
}
