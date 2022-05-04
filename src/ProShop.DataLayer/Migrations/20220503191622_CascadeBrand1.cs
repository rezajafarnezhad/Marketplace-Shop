using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class CascadeBrand1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBrand_Brands_BrandId",
                table: "CategoryBrand");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBrand_Categories_CategoryId",
                table: "CategoryBrand");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryBrand_Brands_BrandId",
                table: "CategoryBrand",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryBrand_Categories_CategoryId",
                table: "CategoryBrand",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBrand_Brands_BrandId",
                table: "CategoryBrand");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBrand_Categories_CategoryId",
                table: "CategoryBrand");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryBrand_Brands_BrandId",
                table: "CategoryBrand",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryBrand_Categories_CategoryId",
                table: "CategoryBrand",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
