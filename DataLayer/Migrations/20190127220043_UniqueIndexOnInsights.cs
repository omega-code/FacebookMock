using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class UniqueIndexOnInsights : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LeadCampaignInsights_LeadCampaignId_Date",
                table: "LeadCampaignInsights",
                columns: new[] { "LeadCampaignId", "Date" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeadCampaignInsights_LeadCampaignId_Date",
                table: "LeadCampaignInsights");
        }
    }
}
