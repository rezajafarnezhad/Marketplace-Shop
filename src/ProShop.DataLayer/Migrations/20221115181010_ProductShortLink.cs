using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class ProductShortLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductShortLinkId",
                table: "Product",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductShortLink",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductShortLink", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductShortLinkId",
                table: "Product",
                column: "ProductShortLinkId",
                unique: true,
                filter: "[ProductShortLinkId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductShortLink_ProductShortLinkId",
                table: "Product",
                column: "ProductShortLinkId",
                principalTable: "ProductShortLink",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductShortLink_ProductShortLinkId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "ProductShortLink");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductShortLinkId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductShortLinkId",
                table: "Product");
        }
    }
}
