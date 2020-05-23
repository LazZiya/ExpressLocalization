using LazZiya.ExpressLocalization.DB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SampleProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Cultures table will hold the supported cultures entities
        public DbSet<ExpressLocalizationCulture> Cultures { get; set; }

        // All localized resources will be saved in this table
        public DbSet<ExpressLocalizationEntity> LocalizationResources { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ExpressLocalizationEntity>()
                .HasOne(r => r.Culture)
                .WithMany(c => c.Resources)
                .OnDelete(DeleteBehavior.Cascade);

            builder.SeedCultures();
            builder.SeedLocalizedData();

            base.OnModelCreating(builder);
        }
    }
}
