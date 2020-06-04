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
                new XLTranslation { ID = 1, ResourceID = 1, CultureID = "tr", Value = "Hoşgeldiniz", IsActive = true },
                new XLTranslation { ID = 2, ResourceID = 2, CultureID = "tr", Value = "Anasayfa", IsActive = true },
                new XLTranslation { ID = 3, ResourceID = 3, CultureID = "tr", Value = "Gizlilik", IsActive = true },
                new XLTranslation { ID = 4, ResourceID = 1, CultureID = "ar", Value = "أهلا و سهلا", IsActive = true },
                new XLTranslation { ID = 5, ResourceID = 2, CultureID = "ar", Value = "الرئيسية", IsActive = true },
                new XLTranslation { ID = 6, ResourceID = 3, CultureID = "ar", Value = "الخصوصية", IsActive = true }
            );
        }
    }
}
