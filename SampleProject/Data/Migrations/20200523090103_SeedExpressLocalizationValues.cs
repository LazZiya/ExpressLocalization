using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleProject.Data.Migrations
{
    public partial class SeedExpressLocalizationValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cultures",
                columns: new[] { "ID", "EnglishName", "IsActive", "IsDefault" },
                values: new object[] { "en", "English", true, true });

            migrationBuilder.InsertData(
                table: "Cultures",
                columns: new[] { "ID", "EnglishName", "IsActive", "IsDefault" },
                values: new object[] { "tr", "Turkish", true, false });

            migrationBuilder.InsertData(
                table: "Cultures",
                columns: new[] { "ID", "EnglishName", "IsActive", "IsDefault" },
                values: new object[] { "ar", "Arabic", true, false });

            migrationBuilder.InsertData(
                table: "LocalizationResources",
                columns: new[] { "ID", "Comment", "CultureName", "Key", "Value" },
                values: new object[,]
                {
                    { 1, null, "tr", "Welcome", "Hoşgeldiniz" },
                    { 2, null, "tr", "Home", "Anasayfa" },
                    { 3, null, "tr", "Privacy", "Gizlilik" },
                    { 4, null, "ar", "Welcome", "أهلا و سهلا" },
                    { 5, null, "ar", "Home", "الرئيسية" },
                    { 6, null, "ar", "Privacy", "الخصوصية" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cultures",
                keyColumn: "ID",
                keyValue: "en");

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cultures",
                keyColumn: "ID",
                keyValue: "ar");

            migrationBuilder.DeleteData(
                table: "Cultures",
                keyColumn: "ID",
                keyValue: "tr");
        }
    }
}
