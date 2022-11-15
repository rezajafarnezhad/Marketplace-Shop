using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class Features : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowNextToProduct",
                table: "Features",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ProductComment_ProductId",
                table: "ProductComment",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_Product_ProductId",
                table: "ProductComment",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_Product_ProductId",
                table: "ProductComment");

            migrationBuilder.DropIndex(
                name: "IX_ProductComment_ProductId",
                table: "ProductComment");

            migrationBuilder.DropColumn(
                name: "ShowNextToProduct",
                table: "Features");
        }
    }
}
