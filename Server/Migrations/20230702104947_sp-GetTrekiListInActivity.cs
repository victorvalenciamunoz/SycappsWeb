using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SycappsWeb.Server.Migrations
{
    public partial class spGetTrekiListInActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"Create procedure GetTrekiListInActivity
	                        @IdActividad int
	                AS
	                BEGIN
	                select T.* 
		                from trekis T
		                inner join [dbo].[ActividadTreki] A On TrekiId = T.Id and A.ActividadesId = @IdActividad
	                END";
            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[GetTrekiListInActivity]";

            migrationBuilder.Sql(sp);
        }
    }
}
