using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class InsightsLoaderStateAndSoOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsightsLoaderState",
                columns: table => new
                {
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    UniversalTimeTicks = table.Column<long>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    AdAccountId = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Success = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsightsLoaderState", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsightsLoaderState_AdAccountId_Date",
                table: "InsightsLoaderState",
                columns: new[] { "AdAccountId", "Date" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsightsLoaderState");
        }
    }
}
