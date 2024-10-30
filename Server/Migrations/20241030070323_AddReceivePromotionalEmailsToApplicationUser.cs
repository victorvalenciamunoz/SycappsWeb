using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SycappsWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddReceivePromotionalEmailsToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReceivePromotionalEmails",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceivePromotionalEmails",
                table: "AspNetUsers");
        }
    }
}
