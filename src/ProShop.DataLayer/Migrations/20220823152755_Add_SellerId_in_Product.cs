using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class Add_SellerId_in_Product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SelerId",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Product_SelerId",
                table: "Product",
                column: "SelerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Sellers_SelerId",
                table: "Product",
                column: "SelerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Sellers_SelerId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_SelerId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SelerId",
                table: "Product");
        }
    }
}
