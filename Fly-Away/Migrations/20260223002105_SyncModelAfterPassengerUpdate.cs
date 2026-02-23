using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly_Away.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelAfterPassengerUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PassengerName",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PassportNumber",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "PassengerName",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Ticket");
        }
    }
}
