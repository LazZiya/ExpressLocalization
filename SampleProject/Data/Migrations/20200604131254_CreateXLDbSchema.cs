using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleProject.Data.Migrations
{
    public partial class CreateXLDbSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "XLCultures",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XLCultures", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "XLResources",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XLResources", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "XLTranslations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CultureID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ResourceID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XLTranslations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_XLTranslations_XLCultures_CultureID",
                        column: x => x.CultureID,
                        principalTable: "XLCultures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_XLTranslations_XLResources_ResourceID",
                        column: x => x.ResourceID,
                        principalTable: "XLResources",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_XLTranslations_CultureID",
                table: "XLTranslations",
                column: "CultureID");

            migrationBuilder.CreateIndex(
                name: "IX_XLTranslations_ResourceID",
                table: "XLTranslations",
                column: "ResourceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "XLTranslations");

            migrationBuilder.DropTable(
                name: "XLCultures");

            migrationBuilder.DropTable(
                name: "XLResources");
        }
    }
}
