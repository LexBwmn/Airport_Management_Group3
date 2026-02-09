using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly_Away.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TicketImageUrl",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketImageUrl",
                table: "Ticket");
        }
    }
}
