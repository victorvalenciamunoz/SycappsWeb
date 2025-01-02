using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SycappsWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionToPuntodeInteres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "PuntosdeInteres",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "PuntosdeInteres");
        }
    }
}
