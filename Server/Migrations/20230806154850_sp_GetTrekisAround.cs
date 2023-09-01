using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SycappsWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class sp_GetTrekisAround : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"Create procedure GetTrekisAround
	                    @currentLatitude float,
	                    @currentLongitude float,
	                    @threshold int
                    as
                    BEGIN
	                    DECLARE @source geography = geography::Point(@currentLatitude, @currentLongitude, 4326);    

	                    select trekis.*
	                    from trekis
	                    where @source.STDistance(geography::Point(latitud, longitud, 4326))<= @threshold
                    end";
            migrationBuilder.Sql(sp);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[GetTrekisAround]";

            migrationBuilder.Sql(sp);
        }
    }
}
