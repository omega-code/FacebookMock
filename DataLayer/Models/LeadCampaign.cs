using System;
using Bogus;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Models
{
    public class LeadCampaign : BaseDataObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
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

            var faker = new Faker<LeadCampaign>()
                .RuleFor(r => r.Id, () => Guid.NewGuid())
                .RuleFor(r => r.Name, f => f.Random.Word());

            var elements = faker.Generate(20);
            context.AddRange(elements);
            context.SaveChanges();
            return elements.Count;
        }
    }
}
