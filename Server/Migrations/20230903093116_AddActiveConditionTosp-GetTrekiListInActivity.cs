using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SycappsWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveConditionTospGetTrekiListInActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"ALTER procedure [dbo].[GetTrekiListInActivity]
	                        @IdActividad int
	                AS
	                BEGIN
	                select T.* 
		                from trekis T
		                inner join [dbo].[ActividadTreki] A On TrekiId = T.Id and A.ActividadesId = @IdActividad
						where t.Activo = 1
	                END";
            migrationBuilder.Sql(sp);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
