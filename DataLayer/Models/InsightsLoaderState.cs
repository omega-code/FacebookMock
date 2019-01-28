using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Models
{
    public class InsightsLoaderState : BaseDataObject
    {
        public Guid Id { get; set; }
        public long AdAccountId { get; set; }
        public DateTime Date { get; set; }
        public bool Success { get; set; }
    }

    public class InsightsLoaderStateConfiguration : BaseConfiguration<InsightsLoaderState>
    {
        public override void Configure(EntityTypeBuilder<InsightsLoaderState> builder)
        {
            base.Configure(builder);

            builder.HasKey(r => r.Id);
            builder.HasIndex(r => new { r.AdAccountId, r.Date }).IsUnique();
        }
    }
}