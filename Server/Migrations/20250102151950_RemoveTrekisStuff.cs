using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SycappsWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTrekisStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActividadTreki");

            migrationBuilder.DropTable(
                name: "CapturaTrekis");

            migrationBuilder.DropTable(
                name: "Actividades");

            migrationBuilder.DropTable(
                name: "Trekis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actividades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValidoDesde = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidoHasta = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actividades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trekis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Latitud = table.Column<double>(type: "float", nullable: false),
                    Longitud = table.Column<double>(type: "float", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trekis", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "CapturaTrekis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActividadId = table.Column<int>(type: "int", nullable: false),
                    TrekiId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaCaptura = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapturaTrekis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CapturaTrekis_Actividades_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "Actividades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CapturaTrekis_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CapturaTrekis_Trekis_TrekiId",
                        column: x => x.TrekiId,
                        principalTable: "Trekis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActividadTreki_TrekiId",
                table: "ActividadTreki",
                column: "TrekiId");

            migrationBuilder.CreateIndex(
                name: "IX_CapturaTrekis_ActividadId",
                table: "CapturaTrekis",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_CapturaTrekis_TrekiId",
                table: "CapturaTrekis",
                column: "TrekiId");

            migrationBuilder.CreateIndex(
                name: "IX_CapturaTrekis_UsuarioId",
                table: "CapturaTrekis",
                column: "UsuarioId");
        }
    }
}
