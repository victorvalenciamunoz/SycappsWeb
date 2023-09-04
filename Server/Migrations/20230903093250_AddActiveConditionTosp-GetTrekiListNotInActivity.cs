using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SycappsWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveConditionTospGetTrekiListNotInActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"ALTER procedure [dbo].[GetTrekiListNotInActivity]
	                        @IdActividad int
	                AS
	                BEGIN
	                select T.* 
		                from trekis T
		                left join [dbo].[ActividadTreki] A On TrekiId = T.Id and A.ActividadesId = @IdActividad
						Where A.ActividadesId is null and T.Activo = 1
	                END";
            migrationBuilder.Sql(sp);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
