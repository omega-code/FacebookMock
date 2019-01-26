using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_Sku",
                table: "Products",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Title",
                table: "Products",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IsGroup",
                table: "Categories",
                column: "IsGroup");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Title",
                table: "Categories",
                column: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Sku",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Title",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_IsGroup",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Title",
                table: "Categories");
        }
    }
}
