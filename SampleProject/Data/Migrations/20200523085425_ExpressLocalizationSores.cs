using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleProject.Data.Migrations
{
    public partial class ExpressLocalizationSores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cultures",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cultures", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LocalizationResources",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CultureName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationResources", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LocalizationResources_Cultures_CultureName",
                        column: x => x.CultureName,
                        principalTable: "Cultures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationResources_CultureName",
                table: "LocalizationResources",
                column: "CultureName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalizationResources");

            migrationBuilder.DropTable(
                name: "Cultures");
        }
    }
}
