namespace Fly_Away.ViewModels;

public class FlightViewModel
{
    public int Flight_ID { get; set; }
    public string AirlineName { get; set; } = "";
    public string SourceName { get; set; } = "";
    public string DestinationName { get; set; } = "";
    public DateOnly DateDeparture { get; set; }
    public TimeOnly TimeDeparture { get; set; }
}
