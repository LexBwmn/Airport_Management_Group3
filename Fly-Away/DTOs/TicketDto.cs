namespace Fly_Away.DTOs;

public class TicketDto
{
    public int Ticket_ID { get; set; }
    public long Barcode { get; set; }

    public int Flight_ID { get; set; }
    public string AirlineName { get; set; } = string.Empty;

    public string SourceName { get; set; } = string.Empty;
    public string DestinationName { get; set; } = string.Empty;

    public DateOnly DateDeparture { get; set; }
    public TimeOnly TimeDeparture { get; set; }
    public DateOnly DateArrival { get; set; }
    public TimeOnly TimeArrival { get; set; }

    public string FlightClass { get; set; } = string.Empty; // Economy/Business/First
    public string BaggageSize { get; set; } = string.Empty;

    public int SeatNum { get; set; }
    public string SeatType { get; set; } = string.Empty; // Window/Middle/Aisle

    public string GateStatus { get; set; } = string.Empty;

    public string TicketImageUrl { get; set; } = string.Empty; // placeholder for now
}
