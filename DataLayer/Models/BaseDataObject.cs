using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DataLayer.Models
{
    public class BaseDataObject
    {
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        public long UniversalTimeTicks { get; set; }
    }


    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseDataObject
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(e => e.UniversalTimeTicks).IsConcurrencyToken();
        }
    }
}
