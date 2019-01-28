using System;
using System.Collections.Generic;
using Bogus;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Models
{
    public class FacebookPage : BaseDataObject
    {
        public Guid Id { get; set; }
        public long AdAccountId { get; set; }
        public bool Deactivated { get; set; }
    }

    public class FacebookPageConfiguration : BaseConfiguration<FacebookPage>, ISeedable
    {
        public override void Configure(EntityTypeBuilder<FacebookPage> builder)
        {
            base.Configure(builder);

            builder.HasKey(r => r.Id);

        }

        public int Seed(AppDbContext context)
        {
            var accountIds = new List<long>();
            var bog = new Faker();
            for (int i = 0; i < 5; i++)
            {
                accountIds.Add(bog.Random.Long(0, 50000));
            }
            

            var faker = new Faker<FacebookPage>()
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.AdAccountId, f => f.PickRandom(accountIds))
                .RuleFor(r => r.Deactivated, f => false);

            var elements = faker.Generate(18);

            context.AddRange(elements);
            context.SaveChanges();
            return elements.Count;
        }
    }
}
