using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Models
{
    public class LeadCampaignInsights : BaseDataObject
    {
        public Guid Id { get; set; }
        public LeadCampaign LeadCampaign { get; set; }
        public Guid LeadCampaignId { get; set; }
        public long AdAccountId { get; set; }
        public uint AmountSpent { get; set; }
        public DateTime Date { get; set; }
    }

    public class LeadCampaignInsightsConfiguration : BaseConfiguration<LeadCampaignInsights>
    {
        public override void Configure(EntityTypeBuilder<LeadCampaignInsights> builder)
        {
            base.Configure(builder);

            builder.HasKey(r => r.Id);
            builder.HasOne(r => r.LeadCampaign).WithMany().HasForeignKey(r => r.LeadCampaignId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);

            builder.HasIndex(r => r.LeadCampaignId);
            builder.HasIndex(r => r.Date);

            builder.HasIndex(r => new { r.LeadCampaignId, r.Date }).IsUnique();
        }

        public List<object> Seed(AppDbContext context, int maxErrorCount, float failProbability)
        {
            var debugLst = new List<object>();

            var startDate = DateTime.Today.AddDays(-30);

            var insightsFaker = new Faker<LeadCampaignInsights>()
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.AmountSpent, f => f.Random.UInt(0, 20000));

            var oldStateData = context.InsightsLoaderState.Where(r => r.Date < startDate);
            context.RemoveRange(oldStateData);
            context.SaveChanges();

            var actualStateData = context.InsightsLoaderState.ToArray();
            var groupedPages = context.FacebookPages
                .Where(r => !r.Deactivated)
                .AsEnumerable()
                .GroupBy(r => r.AdAccountId);
            var totalAccounts = groupedPages.Count();

            var errorCount = 0;

            var lst = new List<LeadCampaignInsights>();
            for (DateTime date = startDate; date <= DateTime.Today; date = date.AddDays(1))
            {
                debugLst.Add($"Started Date {date}....................................");

                var loadedSuccessfully = 0;
                var skipped = 0;
                var failed = 0;
                foreach (var currentAccountId in groupedPages)
                {
                    //debugLst.Add($"Started account {currentAccountId.Key}");
                    if (actualStateData.Where(r => r.Success && r.AdAccountId == currentAccountId.Key && r.Date == date).Count() > 0)
                    {
                        //debugLst.Add($"Account id {currentAccountId.Key} for date {date} is already loaded. Skipping...");
                        skipped++;
                        continue;
                    }

                    foreach (var currentPage in currentAccountId)
                    {

                        if (errorCount < maxErrorCount && !new Faker().Random.Bool(failProbability))
                        {
                            //debugLst.Add($"Page {currentPage.Id} skipped and deactivated.");
                            SaveInsightsState(date, currentAccountId.Key, false, context);
                            DeactivateFacebookPage(currentPage, context);
                            failed++;
                            errorCount++;
                            continue;
                        }

                        var insights = insightsFaker.Generate();
                        insights.AdAccountId = currentPage.AdAccountId;
                        insights.LeadCampaign = context.LeadCampaigns.Where(r => r.FacebookPageId == currentPage.Id).OrderBy(r => Guid.NewGuid()).First();
                        insights.Date = date;
                        //debugLst.Add($"Adding insights to database...");
                        loadedSuccessfully++;
                        //debugLst.Add(insights);
                        context.LeadCampaignInsights.Add(insights);
                        context.SaveChanges();
                        SaveInsightsState(date, currentAccountId.Key, true, context);
                        break;
                    }
                }
                debugLst.Add($"Total: {totalAccounts}, Loaded from API: {loadedSuccessfully}, Already been loaded: {skipped}, Failed pages: {failed}");
            }

            var data = new { Insights = context.LeadCampaignInsights.ToArray(), LoaderState = context.InsightsLoaderState.ToArray(), Pages = context.FacebookPages.ToArray() };
            //return new { Data = data, Log = debugLst };
            return debugLst;
        }

        private void SaveInsightsState(DateTime date, long adAccountId, bool success, AppDbContext context)
        {
            var createNew = false;
            var state = context.InsightsLoaderState.FirstOrDefault(r => r.Date == date && r.AdAccountId == adAccountId);

            if (state == null)
            {
                state = new InsightsLoaderState
                {
                    Id = Guid.NewGuid(),
                    AdAccountId = adAccountId,
                    Date = date,
                };
                createNew = true;
            }

            state.Success = success;

            if (createNew)
            {
                context.InsightsLoaderState.Add(state);
            }
            else
            {
                context.InsightsLoaderState.Update(state);
            }

            context.SaveChanges();
        }

        private void DeactivateFacebookPage(FacebookPage page, AppDbContext context)
        {
            page.Deactivated = true;
            context.FacebookPages.Update(page);
            context.SaveChanges();
        }
    }
}
