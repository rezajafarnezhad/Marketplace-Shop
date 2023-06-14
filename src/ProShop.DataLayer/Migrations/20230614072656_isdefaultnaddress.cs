using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class isdefaultnaddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Address",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Address");
        }
    }
}
