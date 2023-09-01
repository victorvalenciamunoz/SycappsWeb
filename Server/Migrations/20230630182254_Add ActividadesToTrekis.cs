using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SycappsWeb.Server.Migrations
{
    public partial class AddActividadesToTrekis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActividadTreki",
                columns: table => new
                {
                    ActividadesId = table.Column<int>(type: "int", nullable: false),
                    TrekiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActividadTreki", x => new { x.ActividadesId, x.TrekiId });
                    table.ForeignKey(
                        name: "FK_ActividadTreki_Actividades_ActividadesId",
                        column: x => x.ActividadesId,
                        principalTable: "Actividades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActividadTreki_Trekis_TrekiId",
                        column: x => x.TrekiId,
                        principalTable: "Trekis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActividadTreki_TrekiId",
                table: "ActividadTreki",
                column: "TrekiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActividadTreki");
        }
    }
}
