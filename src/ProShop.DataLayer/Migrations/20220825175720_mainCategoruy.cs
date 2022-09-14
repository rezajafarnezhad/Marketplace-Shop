using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class mainCategoruy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MainCategoryId",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Product_MainCategoryId",
                table: "Product",
                column: "MainCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Categories_MainCategoryId",
                table: "Product",
                column: "MainCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Categories_MainCategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_MainCategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "MainCategoryId",
                table: "Product");
        }
    }
}
