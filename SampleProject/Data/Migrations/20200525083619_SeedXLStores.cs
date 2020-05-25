using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleProject.Data.Migrations
{
    public partial class SeedXLStores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "XLCultures",
                columns: new[] { "ID", "EnglishName", "IsActive", "IsDefault" },
                values: new object[,]
                {
                    { "en", "English", true, true },
                    { "tr", "Turkish", true, false },
                    { "ar", "Arabic", true, false }
                });

            migrationBuilder.InsertData(
                table: "XLResources",
                columns: new[] { "ID", "Comment", "Key" },
                values: new object[,]
                {
                    { 1, null, "Welcome" },
                    { 2, null, "Home" },
                    { 3, null, "Privacy" }
                });

            migrationBuilder.InsertData(
                table: "XLTranslations",
                columns: new[] { "ID", "CultureName", "ResourceID", "Value" },
                values: new object[,]
                {
                    { 1, "tr", 1, "Hoşgeldiniz" },
                    { 4, "ar", 1, "أهلا و سهلا" },
                    { 2, "tr", 2, "Anasayfa" },
                    { 5, "ar", 2, "الرئيسية" },
                    { 3, "tr", 3, "Gizlilik" },
                    { 6, "ar", 3, "الخصوصية" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "XLCultures",
                keyColumn: "ID",
                keyValue: "en");

            migrationBuilder.DeleteData(
                table: "XLTranslations",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "XLTranslations",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "XLTranslations",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "XLTranslations",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "XLTranslations",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "XLTranslations",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "XLCultures",
                keyColumn: "ID",
                keyValue: "ar");

            migrationBuilder.DeleteData(
                table: "XLCultures",
                keyColumn: "ID",
                keyValue: "tr");

            migrationBuilder.DeleteData(
                table: "XLResources",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "XLResources",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "XLResources",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}
