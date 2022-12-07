using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class EditProductVariant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "ProductVariant",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OffPrice",
                table: "ProductVariant",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTime",
                table: "ProductVariant",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "offPercentage",
                table: "ProductVariant",
                type: "tinyint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "OffPrice",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "offPercentage",
                table: "ProductVariant");
        }
    }
}
