using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class uj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBrand_Brands_BrandId",
                table: "CategoryBrand");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBrand_Categories_CategoryId",
                table: "CategoryBrand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryBrand",
                table: "CategoryBrand");

            migrationBuilder.DropIndex(
                name: "IX_CategoryBrand_BrandId",
                table: "CategoryBrand");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CategoryBrand");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CategoryBrand");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryBrand",
                table: "CategoryBrand",
                columns: new[] { "BrandId", "CategoryId" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBrand_Brands_BrandId",
                table: "CategoryBrand");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBrand_Categories_CategoryId",
                table: "CategoryBrand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryBrand",
                table: "CategoryBrand");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "CategoryBrand",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CategoryBrand",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryBrand",
                table: "CategoryBrand",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBrand_BrandId",
                table: "CategoryBrand",
                column: "BrandId");

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
    }
}
