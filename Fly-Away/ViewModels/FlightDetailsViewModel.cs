namespace Fly_Away.ViewModels;

public class FlightDetailsViewModel
{
    public int Flight_ID { get; set; }

    public string AirlineName { get; set; } = "";
    public string SourceName { get; set; } = "";
    public string DestinationName { get; set; } = "";

    public DateOnly DateDeparture { get; set; }
    public TimeOnly TimeDeparture { get; set; }

    public DateOnly DateArrival { get; set; }
    public TimeOnly TimeArrival { get; set; }

    public List<FlightClassItem> Classes { get; set; } = new();
}

public class FlightClassItem
{
    public int FlightClass_ID { get; set; }
    public string SeatType { get; set; } = "";
    public string BaggageSize { get; set; } = "";
}
