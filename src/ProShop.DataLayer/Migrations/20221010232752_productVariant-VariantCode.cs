using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class productVariantVariantCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VariantCode",
                table: "ProductVariant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_VariantCode",
                table: "ProductVariant",
                column: "VariantCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductVariant_VariantCode",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "VariantCode",
                table: "ProductVariant");
        }
    }
}
