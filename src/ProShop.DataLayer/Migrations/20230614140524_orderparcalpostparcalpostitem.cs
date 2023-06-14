using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class orderparcalpostparcalpostitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AddressId = table.Column<long>(type: "bigint", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayFromWallet = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParcalPosts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Dimensions = table.Column<byte>(type: "tinyint", nullable: false),
                    OrderStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    PostTrackingCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ShippingPrice = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcalPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParcalPosts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParcelPostItems",
                columns: table => new
                {
                    ParcalPostId = table.Column<long>(type: "bigint", nullable: false),
                    ProductVariantId = table.Column<long>(type: "bigint", nullable: false),
                    GaranteeId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    DiscountPrice = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelPostItems", x => new { x.ParcalPostId, x.ProductVariantId });
                    table.ForeignKey(
                        name: "FK_ParcelPostItems_Garantee_GaranteeId",
                        column: x => x.GaranteeId,
                        principalTable: "Garantee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParcelPostItems_ParcalPosts_ParcalPostId",
                        column: x => x.ParcalPostId,
                        principalTable: "ParcalPosts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParcelPostItems_ProductVariant_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcalPosts_OrderId",
                table: "ParcalPosts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcalPosts_PostTrackingCode",
                table: "ParcalPosts",
                column: "PostTrackingCode",
                unique: true,
                filter: "[PostTrackingCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelPostItems_GaranteeId",
                table: "ParcelPostItems",
                column: "GaranteeId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelPostItems_ProductVariantId",
                table: "ParcelPostItems",
                column: "ProductVariantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParcelPostItems");

            migrationBuilder.DropTable(
                name: "ParcalPosts");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
