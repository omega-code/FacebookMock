using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class FacebookInsights_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FacebookPages",
                columns: table => new
                {
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    UniversalTimeTicks = table.Column<long>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    AdAccountId = table.Column<long>(nullable: false),
                    Deactivated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeadCampaigns",
                columns: table => new
                {
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    UniversalTimeTicks = table.Column<long>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 85, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadCampaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeadCampaignInsights",
                columns: table => new
                {
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    UniversalTimeTicks = table.Column<long>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    LeadCampaignId = table.Column<Guid>(nullable: false),
                    AdAccountId = table.Column<long>(nullable: false),
                    AmountSpent = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadCampaignInsights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadCampaignInsights_LeadCampaigns_LeadCampaignId",
                        column: x => x.LeadCampaignId,
                        principalTable: "LeadCampaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeadCampaignInsights_Date",
                table: "LeadCampaignInsights",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_LeadCampaignInsights_LeadCampaignId",
                table: "LeadCampaignInsights",
                column: "LeadCampaignId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacebookPages");

            migrationBuilder.DropTable(
                name: "LeadCampaignInsights");

            migrationBuilder.DropTable(
                name: "LeadCampaigns");
        }
    }
}
