using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.Metrics;
using System;

#nullable disable

namespace SycappsWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveConditionTospGetTrekisAround : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"ALTER procedure[dbo].[GetTrekisAround]
            @currentLatitude float,
            @currentLongitude float,
            @threshold int
                    as
                    BEGIN
                        DECLARE @source geography = geography::Point(@currentLatitude, @currentLongitude, 4326);
                            select trekis.*
                            from trekis
                            where @source.STDistance(geography::Point(latitud, longitud, 4326)) <= @threshold
                            and trekis.Activo = 1
                    end";
            
            migrationBuilder.Sql(sp);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
