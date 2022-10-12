using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class qqwq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConsignmentItem_ProductVariantId",
                table: "ConsignmentItem");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentItem_ProductVariantId_ConsignmentId",
                table: "ConsignmentItem",
                columns: new[] { "ProductVariantId", "ConsignmentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConsignmentItem_ProductVariantId_ConsignmentId",
                table: "ConsignmentItem");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentItem_ProductVariantId",
                table: "ConsignmentItem",
                column: "ProductVariantId");
        }
    }
}
