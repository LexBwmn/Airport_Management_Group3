namespace Fly_Away.Models;

public class Ticket
{
    public int Ticket_ID { get; set; }
    public long Barcode { get; set; }

    public int Account_ID { get; set; }
    public int FlightClass_ID { get; set; }
    public int Seat_ID { get; set; }
    public int Gate_ID { get; set; }
    public int Flight_ID { get; set; }

    public Account? Account { get; set; }
    public FlightClass? FlightClass { get; set; }
    public Seat? Seat { get; set; }
    public Gate? Gate { get; set; }
    public Flight? Flight { get; set; }

    public string? TicketImageUrl { get; set; }

}
