using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Models
{
    public class Category : BaseDataObject, ISoftDeletable
    {
        public int Id { get; set; }
        public bool SoftDeleted { get; set; }

        [JsonIgnore]
        public Category Parent { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public bool IsGroup { get; set; }
        public ICollection<Category> Children { get; set; } = new List<Category>();
    }

    public class CategoryConfiguration : BaseConfiguration<Category>, ISeedable
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            builder.HasKey(r => r.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(r => r.Title).HasMaxLength(85).IsRequired();
            builder.HasOne(r => r.Parent).WithMany(r => r.Children)
                .HasForeignKey(r => r.ParentId).IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.ParentId);
            builder.HasIndex(c => c.Title);
            builder.HasIndex(c => c.IsGroup);
        }

        public int Seed(AppDbContext context)
        {
            var faker = new Faker<Category>()
                .RuleFor(r => r.IsGroup, () => true)
                .RuleFor(r => r.Parent, () => null)
                .RuleFor(r => r.UniversalTimeTicks, () => DateTime.Now.ToUniversalTime().Ticks)
                .RuleFor(r => r.Title, f => "Folder: " + f.Random.Word());

            var folders1 = faker.Generate(5);

            faker.RuleFor(r => r.Parent, () => folders1.OrderBy(r => Guid.NewGuid()).First());
            var folders2 = faker.Generate(10);
            var folders3 = folders1.Concat(folders2).ToArray();

            faker.RuleFor(r => r.Parent, () => folders3.OrderBy(r => Guid.NewGuid()).First());
            faker.RuleFor(r => r.Title, f => f.Random.Word());
            faker.RuleFor(r => r.IsGroup, () => false);

            var elements = faker.Generate(20);

            var allSeeds = elements.Concat(folders3).ToArray();

            context.AddRange(allSeeds);
            context.SaveChanges();
            return allSeeds.Length;
        }
    }
}
