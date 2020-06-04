using LazZiya.ExpressLocalization.DB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

namespace SampleProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        // Cultures table will hold the supported cultures entities
        public DbSet<XLCulture> XLCultures { get; set; }

        // All resources will be saved in this table
        public DbSet<XLResource> XLResources { get; set; }
        
        // Localized values will be saved in this table
        public DbSet<XLTranslation> XLTranslations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<XLTranslation>()
                .HasOne(t => t.Culture as XLCulture)
                .WithMany(c => c.Translations as IEnumerable<XLTranslation>)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<XLTranslation>()
                .HasOne(t => t.Resource as XLResource)
                .WithMany(r => r.Translations as IEnumerable<XLTranslation>)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.SeedCultures();
            builder.SeedResourceData();
            builder.SeedLocalizedData();

            base.OnModelCreating(builder);
        }
    }
}
