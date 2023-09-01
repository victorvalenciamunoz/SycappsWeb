using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SycappsWeb.Server.Migrations
{
    public partial class spGetTrekiListNotInActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"Create procedure GetTrekiListNotInActivity
	                        @IdActividad int
	                AS
	                BEGIN
	                select T.* 
		                from trekis T
		                left join [dbo].[ActividadTreki] A On TrekiId = T.Id and A.ActividadesId = @IdActividad
						Where A.ActividadesId is null
	                END";
            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[GetTrekiListNotInActivity]";

            migrationBuilder.Sql(sp);
        }
    }
}
