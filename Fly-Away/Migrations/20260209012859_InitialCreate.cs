using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly_Away.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Account_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Account_ID);
                });

            migrationBuilder.CreateTable(
                name: "Airline",
                columns: table => new
                {
                    Airline_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airline", x => x.Airline_ID);
                });

            migrationBuilder.CreateTable(
                name: "Airport",
                columns: table => new
                {
                    Airport_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airport", x => x.Airport_ID);
                });

            migrationBuilder.CreateTable(
                name: "FlightClass",
                columns: table => new
                {
                    FlightClass_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaggageSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightClass", x => x.FlightClass_ID);
                });

            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    Flight_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Destination_ID = table.Column<int>(type: "int", nullable: false),
                    Source_ID = table.Column<int>(type: "int", nullable: false),
                    Airline_ID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDeparture = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeDeparture = table.Column<TimeOnly>(type: "time", nullable: false),
                    DateArrival = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeArrival = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.Flight_ID);
                    table.ForeignKey(
                        name: "FK_Flight_Airline_Airline_ID",
                        column: x => x.Airline_ID,
                        principalTable: "Airline",
                        principalColumn: "Airline_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flight_Airport_Destination_ID",
                        column: x => x.Destination_ID,
                        principalTable: "Airport",
                        principalColumn: "Airport_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flight_Airport_Source_ID",
                        column: x => x.Source_ID,
                        principalTable: "Airport",
                        principalColumn: "Airport_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Gate",
                columns: table => new
                {
                    Gate_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Airport_ID = table.Column<int>(type: "int", nullable: false),
                    Flight_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gate", x => x.Gate_ID);
                    table.ForeignKey(
                        name: "FK_Gate_Airport_Airport_ID",
                        column: x => x.Airport_ID,
                        principalTable: "Airport",
                        principalColumn: "Airport_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gate_Flight_Flight_ID",
                        column: x => x.Flight_ID,
                        principalTable: "Flight",
                        principalColumn: "Flight_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    Seat_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeatNum = table.Column<int>(type: "int", nullable: false),
                    SeatType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Flight_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.Seat_ID);
                    table.ForeignKey(
                        name: "FK_Seat_Flight_Flight_ID",
                        column: x => x.Flight_ID,
                        principalTable: "Flight",
                        principalColumn: "Flight_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Ticket_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<long>(type: "bigint", nullable: false),
                    Account_ID = table.Column<int>(type: "int", nullable: false),
                    FlightClass_ID = table.Column<int>(type: "int", nullable: false),
                    Seat_ID = table.Column<int>(type: "int", nullable: false),
                    Gate_ID = table.Column<int>(type: "int", nullable: false),
                    Flight_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Ticket_ID);
                    table.ForeignKey(
                        name: "FK_Ticket_Account_Account_ID",
                        column: x => x.Account_ID,
                        principalTable: "Account",
                        principalColumn: "Account_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_FlightClass_FlightClass_ID",
                        column: x => x.FlightClass_ID,
                        principalTable: "FlightClass",
                        principalColumn: "FlightClass_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Flight_Flight_ID",
                        column: x => x.Flight_ID,
                        principalTable: "Flight",
                        principalColumn: "Flight_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Gate_Gate_ID",
                        column: x => x.Gate_ID,
                        principalTable: "Gate",
                        principalColumn: "Gate_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Seat_Seat_ID",
                        column: x => x.Seat_ID,
                        principalTable: "Seat",
                        principalColumn: "Seat_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Airline_ID",
                table: "Flight",
                column: "Airline_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Destination_ID",
                table: "Flight",
                column: "Destination_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Source_ID",
                table: "Flight",
                column: "Source_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Gate_Airport_ID",
                table: "Gate",
                column: "Airport_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Gate_Flight_ID",
                table: "Gate",
                column: "Flight_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_Flight_ID",
                table: "Seat",
                column: "Flight_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_Account_ID",
                table: "Ticket",
                column: "Account_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_Flight_ID",
                table: "Ticket",
                column: "Flight_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_FlightClass_ID",
                table: "Ticket",
                column: "FlightClass_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_Gate_ID",
                table: "Ticket",
                column: "Gate_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_Seat_ID",
                table: "Ticket",
                column: "Seat_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "FlightClass");

            migrationBuilder.DropTable(
                name: "Gate");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "Flight");

            migrationBuilder.DropTable(
                name: "Airline");

            migrationBuilder.DropTable(
                name: "Airport");
        }
    }
}
