using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SycappsWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddPuntosdeInteres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PuntosdeInteres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitud = table.Column<double>(type: "float", nullable: false),
                    Longitud = table.Column<double>(type: "float", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Distrito = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Barrio = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Calle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NumFinca = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TipoReserva = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FormaAparcamiento = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    NumPlazas = table.Column<int>(type: "int", nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlImagen = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuntosdeInteres", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PuntosdeInteres");
        }
    }
}
