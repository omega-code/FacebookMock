using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Bogus;
using System.Linq;

namespace DataLayer.Models
{
    public class Product : BaseDataObject, ISoftDeletable
    {
        public int Id { get; set; }
        public bool SoftDeleted { get; set; }
        public string Sku { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class ProductConfiguration : BaseConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Sku).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Title).HasMaxLength(150).IsRequired();
            builder.Property(c => c.CategoryId).IsRequired();
            builder.HasOne(c => c.Category).WithMany().HasForeignKey(f => f.CategoryId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.Sku).IsUnique();
            builder.HasIndex(c => c.Title);
            builder.HasIndex(c => c.CategoryId);
        }

        public int Seed(AppDbContext context)
        {
            var lorem = new Bogus.DataSets.Lorem();

            var faker = new Faker<Product>()
                .RuleFor(r => r.Sku, f => f.Random.AlphaNumeric(8))
                .RuleFor(r => r.Title, f => f.Random.Word())
                .RuleFor(r => r.Category, () => context.Categories.Where(c => !c.IsGroup).OrderBy(o => Guid.NewGuid()).First())
                .RuleFor(r => r.Description, () => lorem.Sentence(14));

            var prod = faker.Generate(50);
            context.AddRange(prod);
            context.SaveChanges();
            return prod.Count;
        }
    }
}
