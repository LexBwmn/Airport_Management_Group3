using System.Net.Sockets;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Fly_Away.Models;

public class Flight
{
    public int Flight_ID { get; set; }

    public int Destination_ID { get; set; }
    public int Source_ID { get; set; }
    public int Airline_ID { get; set; }

    public string Status { get; set; } = "Scheduled";

    public DateOnly DateDeparture { get; set; }
    public TimeOnly TimeDeparture { get; set; }
    public DateOnly DateArrival { get; set; }
    public TimeOnly TimeArrival { get; set; }

    public Airport? Destination { get; set; }
    public Airport? Source { get; set; }
    public Airline? Airline { get; set; }

    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    public ICollection<Gate> Gates { get; set; } = new List<Gate>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
