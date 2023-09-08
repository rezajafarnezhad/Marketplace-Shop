using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProShop.DataLayer.Migrations
{
    public partial class Ans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductQuestionAndAnswer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Body = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    IsUnknown = table.Column<bool>(type: "bit", nullable: false),
                    IsBuyer = table.Column<bool>(type: "bit", nullable: false),
                    IsParent = table.Column<long>(type: "bigint", nullable: true),
                    SellerId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_ProductQuestionAndAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductQuestionAndAnswer_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductQuestionAndAnswer_ProductQuestionAndAnswer_IsParent",
                        column: x => x.IsParent,
                        principalTable: "ProductQuestionAndAnswer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductQuestionAndAnswer_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductQuestionAndAnswer_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductQuestionAnswerScore",
                columns: table => new
                {
                    AnswerId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_ProductQuestionAnswerScore", x => new { x.UserId, x.AnswerId });
                    table.ForeignKey(
                        name: "FK_ProductQuestionAnswerScore_ProductQuestionAndAnswer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "ProductQuestionAndAnswer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductQuestionAnswerScore_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductQuestionAndAnswer_IsParent",
                table: "ProductQuestionAndAnswer",
                column: "IsParent");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQuestionAndAnswer_ProductId",
                table: "ProductQuestionAndAnswer",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQuestionAndAnswer_SellerId",
                table: "ProductQuestionAndAnswer",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQuestionAndAnswer_UserId",
                table: "ProductQuestionAndAnswer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQuestionAnswerScore_AnswerId",
                table: "ProductQuestionAnswerScore",
                column: "AnswerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductQuestionAnswerScore");

            migrationBuilder.DropTable(
                name: "ProductQuestionAndAnswer");
        }
    }
}
