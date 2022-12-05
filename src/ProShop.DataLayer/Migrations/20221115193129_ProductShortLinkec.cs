using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class ProductShortLinkec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductShortLink_ProductShortLinkId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductShortLinkId",
                table: "Product");

            migrationBuilder.AlterColumn<long>(
                name: "ProductShortLinkId",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductShortLinkId",
                table: "Product",
                column: "ProductShortLinkId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductShortLink_ProductShortLinkId",
                table: "Product",
                column: "ProductShortLinkId",
                principalTable: "ProductShortLink",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductShortLink_ProductShortLinkId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductShortLinkId",
                table: "Product");

            migrationBuilder.AlterColumn<long>(
                name: "ProductShortLinkId",
                table: "Product",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

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
    }
}
