using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer
{
    public class AppDbContext : DbContext
    {

        #region tables

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<LeadCampaign> LeadCampaigns { get; set; }
        public DbSet<LeadCampaignInsights> LeadCampaignInsights { get; set; }
        public DbSet<FacebookPage> FacebookPages { get; set; }

        #endregion

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new LeadCampaignConfiguration());
            modelBuilder.ApplyConfiguration(new LeadCampaignInsightsConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookPageConfiguration());
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected void AddTimestamps()
        {

            foreach (var entry in ChangeTracker.Entries().Where(x => x.Entity.GetType().GetProperty(nameof(BaseDataObject.CreatedTime)) != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(BaseDataObject.CreatedTime)).CurrentValue = DateTime.Now.ToUniversalTime();
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(
                e =>
                    e.Entity.GetType().GetProperty(nameof(BaseDataObject.UpdatedTime)) != null &&
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Added))
            {
                entry.Property(nameof(BaseDataObject.UpdatedTime)).CurrentValue = DateTime.Now.ToUniversalTime();
            }

            foreach (var entry in ChangeTracker.Entries().Where(
                e =>
                    e.Entity.GetType().GetProperty(nameof(BaseDataObject.UniversalTimeTicks)) != null &&
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Added))
            {
                entry.Property(nameof(BaseDataObject.UniversalTimeTicks)).CurrentValue = DateTime.Now.ToUniversalTime().Ticks;
            }
        }
    }
}
