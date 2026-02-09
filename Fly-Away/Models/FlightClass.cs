namespace Fly_Away.Models;

public class FlightClass
{
    public int FlightClass_ID { get; set; }
    public string BaggageSize { get; set; } = string.Empty; // Small/Medium/Large
    public string SeatType { get; set; } = string.Empty;    // Economy/Business/First
}
