using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class commentProduct2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_Sellers_SellerShopNameId",
                table: "ProductComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_Variant_VariantId",
                table: "ProductComment");

            migrationBuilder.AlterColumn<long>(
                name: "VariantId",
                table: "ProductComment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "SellerShopNameId",
                table: "ProductComment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_Sellers_SellerShopNameId",
                table: "ProductComment",
                column: "SellerShopNameId",
                principalTable: "Sellers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_Variant_VariantId",
                table: "ProductComment",
                column: "VariantId",
                principalTable: "Variant",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_Sellers_SellerShopNameId",
                table: "ProductComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_Variant_VariantId",
                table: "ProductComment");

            migrationBuilder.AlterColumn<long>(
                name: "VariantId",
                table: "ProductComment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SellerShopNameId",
                table: "ProductComment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_Sellers_SellerShopNameId",
                table: "ProductComment",
                column: "SellerShopNameId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_Variant_VariantId",
                table: "ProductComment",
                column: "VariantId",
                principalTable: "Variant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
