using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleProject.Data.Migrations
{
    public partial class ExRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalizationResources_Cultures_CultureName",
                table: "LocalizationResources");

            migrationBuilder.AddForeignKey(
                name: "FK_LocalizationResources_Cultures_CultureName",
                table: "LocalizationResources",
                column: "CultureName",
                principalTable: "Cultures",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalizationResources_Cultures_CultureName",
                table: "LocalizationResources");

            migrationBuilder.AddForeignKey(
                name: "FK_LocalizationResources_Cultures_CultureName",
                table: "LocalizationResources",
                column: "CultureName",
                principalTable: "Cultures",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
