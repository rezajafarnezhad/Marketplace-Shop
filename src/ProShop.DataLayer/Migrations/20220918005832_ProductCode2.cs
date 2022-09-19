using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class ProductCode2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CategoryBrand_BrandId",
                table: "CategoryBrand");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductCode",
                table: "Product",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBrand_BrandId_CategoryId",
                table: "CategoryBrand",
                columns: new[] { "BrandId", "CategoryId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_ProductCode",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_CategoryBrand_BrandId_CategoryId",
                table: "CategoryBrand");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBrand_BrandId",
                table: "CategoryBrand",
                column: "BrandId");
        }
    }
}
