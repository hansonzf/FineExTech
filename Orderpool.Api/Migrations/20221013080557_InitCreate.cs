using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orderpool.Api.Migrations
{
    public partial class InitCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderWatchers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginOrderPK = table.Column<int>(type: "int", nullable: false),
                    Handler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessingStartTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderWatchers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RemoteOrder",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginOrderPK = table.Column<int>(type: "int", nullable: false),
                    OrderUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentOfOrder = table.Column<string>(type: "text", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemoteOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderProcess",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WatcherId = table.Column<long>(type: "bigint", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false),
                    ProcessTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderWatcherId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProcess", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_OrderProcess_OrderWatchers_OrderWatcherId",
                        column: x => x.OrderWatcherId,
                        principalTable: "OrderWatchers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderProcess_OrderWatchers_WatcherId",
                        column: x => x.WatcherId,
                        principalTable: "OrderWatchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProcess_OrderWatcherId",
                table: "OrderProcess",
                column: "OrderWatcherId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProcess_WatcherId",
                table: "OrderProcess",
                column: "WatcherId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderWatchers_OrderUuid",
                table: "OrderWatchers",
                column: "OrderUuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RemoteOrder_OrderUuid",
                table: "RemoteOrder",
                column: "OrderUuid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProcess");

            migrationBuilder.DropTable(
                name: "RemoteOrder");

            migrationBuilder.DropTable(
                name: "OrderWatchers");
        }
    }
}
