using System;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Models
{
    public class LeadCampaign : BaseDataObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public FacebookPage FacebookPage { get; set; }
        public Guid FacebookPageId { get; set; }
    }

    public class LeadCampaignConfiguration : BaseConfiguration<LeadCampaign>, ISeedable
    {
        public override void Configure(EntityTypeBuilder<LeadCampaign> builder)
        {
            base.Configure(builder);

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Name).HasMaxLength(85).IsRequired();
        }

        public int Seed(AppDbContext context)
        {
            var pages = context.FacebookPages.ToArray();

            var faker = new Faker<LeadCampaign>()
                .RuleFor(r => r.Id, () => Guid.NewGuid())
                .RuleFor(r => r.Name, f => f.Random.Word());

            int totalCount = 0;
            foreach(var page in pages)
            {
                for (int i = 0; i < 2; i++)
                {
                    var campaign = faker.Generate();
                    campaign.FacebookPage = page;
                    context.Add(campaign);
                    totalCount++;
                }
            }

            context.SaveChanges();
            return totalCount;
        }
    }
}
