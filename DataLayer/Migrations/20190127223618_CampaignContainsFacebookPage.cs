using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class CampaignContainsFacebookPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FacebookPageId",
                table: "LeadCampaigns",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LeadCampaigns_FacebookPageId",
                table: "LeadCampaigns",
                column: "FacebookPageId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadCampaigns_FacebookPages_FacebookPageId",
                table: "LeadCampaigns",
                column: "FacebookPageId",
                principalTable: "FacebookPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadCampaigns_FacebookPages_FacebookPageId",
                table: "LeadCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_LeadCampaigns_FacebookPageId",
                table: "LeadCampaigns");

            migrationBuilder.DropColumn(
                name: "FacebookPageId",
                table: "LeadCampaigns");
        }
    }
}
