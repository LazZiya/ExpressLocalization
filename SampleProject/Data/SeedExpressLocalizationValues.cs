using LazZiya.ExpressLocalization.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace SampleProject.Data
{
    public static class SeedExpressLocalizationValues
    {
        public static void SeedCultures(this ModelBuilder modelBuilder)
        {
            // Seed initial data for localization stores
            modelBuilder.Entity<ExpressLocalizationCulture>().HasData(
                new ExpressLocalizationCulture { IsActive = true, IsDefault = true, ID = "en", EnglishName = "English" },
                new ExpressLocalizationCulture { IsActive = true, IsDefault = false, ID = "tr", EnglishName = "Turkish" },
                new ExpressLocalizationCulture { IsActive = true, IsDefault = false, ID = "ar", EnglishName = "Arabic" }
                );
        }

        public static void SeedLocalizedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExpressLocalizationEntity>().HasData(
                new ExpressLocalizationEntity { ID = 1, CultureName = "tr", Key = "Welcome", Value = "Hoşgeldiniz" },
                new ExpressLocalizationEntity { ID = 2, CultureName = "tr", Key = "Home", Value = "Anasayfa" },
                new ExpressLocalizationEntity { ID = 3, CultureName = "tr", Key = "Privacy", Value = "Gizlilik" },
                new ExpressLocalizationEntity { ID = 4, CultureName = "ar", Key = "Welcome", Value = "أهلا و سهلا" },
                new ExpressLocalizationEntity { ID = 5, CultureName = "ar", Key = "Home", Value = "الرئيسية" },
                new ExpressLocalizationEntity { ID = 6, CultureName = "ar", Key = "Privacy", Value = "الخصوصية" }
            );
        }
    }
}
