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

    public class LeadCampaignInsightsConfiguration : BaseConfiguration<LeadCampaignInsights>, ISeedable
    {
        public override void Configure(EntityTypeBuilder<LeadCampaignInsights> builder)
        {
            base.Configure(builder);

            builder.HasKey(r => r.Id);
            builder.HasOne(r => r.LeadCampaign).WithMany().HasForeignKey(r => r.LeadCampaignId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);

            builder.HasIndex(r => r.LeadCampaignId);
            builder.HasIndex(r => r.Date);
        }

        public int Seed(AppDbContext context)
        {
            var date = DateTime.Today.AddDays(-30);
            var booler = new Faker();

            var faker = new Faker<LeadCampaignInsights>()
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.LeadCampaign, f => context.LeadCampaigns.OrderBy(r => Guid.NewGuid()).First())
                .RuleFor(r => r.AmountSpent, f => f.Random.UInt(0, 20000))
                .RuleFor(r => r.Date, f => date);

            var lst = new List<LeadCampaignInsights>();
            while (date <= DateTime.Today)
            {
                // Чистим таблицу InsightsLoaderState (всё что старше 30 дней)
                // Группируем страницы по adAccountId
                // Внутри каждой группы либо грузим инсайты, либо вылетаем с ошибкой и отключаем страницу.
                // После загрузки инсайтов пишемся в insightsLoaderState и переходим к следующему AdAccountId, независимо от количества оставшихся страниц.
                // Если в стейте по данному AdAccountId есть успешная запись, переходим к следующему AdAccountId.


                lst.Add(faker.Generate());
                date = date.AddDays(1);
            }

            context.AddRange(lst);
            context.SaveChanges();
            return lst.Count;
        }
    }
}
