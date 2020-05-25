using LazZiya.ExpressLocalization.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace SampleProject.Data
{
    public static class SeedExpressLocalizationValues
    {
        public static void SeedCultures(this ModelBuilder modelBuilder)
        {
            // Seed initial data for localization stores
            modelBuilder.Entity<XLCulture>().HasData(
                new XLCulture { IsActive = true, IsDefault = true, ID = "en", EnglishName = "English" },
                new XLCulture { IsActive = true, IsDefault = false, ID = "tr", EnglishName = "Turkish" },
                new XLCulture { IsActive = true, IsDefault = false, ID = "ar", EnglishName = "Arabic" }
                );
        }

        public static void SeedResourceData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<XLResource>().HasData(
                new XLResource { ID = 1, Key = "Welcome" },
                new XLResource { ID = 2, Key = "Home", },
                new XLResource { ID = 3, Key = "Privacy" }
            );
        }

        public static void SeedLocalizedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<XLTranslation>().HasData(
                new XLTranslation { ID = 1, ResourceID = 1, CultureName = "tr", Value = "Hoşgeldiniz" },
                new XLTranslation { ID = 2, ResourceID = 2, CultureName = "tr", Value = "Anasayfa" },
                new XLTranslation { ID = 3, ResourceID = 3, CultureName = "tr", Value = "Gizlilik" },
                new XLTranslation { ID = 4, ResourceID = 1, CultureName = "ar", Value = "أهلا و سهلا" },
                new XLTranslation { ID = 5, ResourceID = 2, CultureName = "ar", Value = "الرئيسية" },
                new XLTranslation { ID = 6, ResourceID = 3, CultureName = "ar", Value = "الخصوصية" }
            );
        }
    }
}
