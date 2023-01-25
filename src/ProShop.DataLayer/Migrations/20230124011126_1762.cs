using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class _1762 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariant_Variant_VariantId",
                table: "ProductVariant");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariant_SellerId_ProductId_VariantId",
                table: "ProductVariant");

            migrationBuilder.AlterColumn<long>(
                name: "VariantId",
                table: "ProductVariant",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_SellerId_ProductId_VariantId",
                table: "ProductVariant",
                columns: new[] { "SellerId", "ProductId", "VariantId" },
                unique: true,
                filter: "[VariantId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariant_Variant_VariantId",
                table: "ProductVariant",
                column: "VariantId",
                principalTable: "Variant",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariant_Variant_VariantId",
                table: "ProductVariant");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariant_SellerId_ProductId_VariantId",
                table: "ProductVariant");

            migrationBuilder.AlterColumn<long>(
                name: "VariantId",
                table: "ProductVariant",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_SellerId_ProductId_VariantId",
                table: "ProductVariant",
                columns: new[] { "SellerId", "ProductId", "VariantId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariant_Variant_VariantId",
                table: "ProductVariant",
                column: "VariantId",
                principalTable: "Variant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
