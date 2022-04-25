using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class DocumentSeller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDocumentApproved",
                table: "Sellers");

            migrationBuilder.AddColumn<byte>(
                name: "DocumentStatus",
                table: "Sellers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentStatus",
                table: "Sellers");

            migrationBuilder.AddColumn<bool>(
                name: "IsDocumentApproved",
                table: "Sellers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
