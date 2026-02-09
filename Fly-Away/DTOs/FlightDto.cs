namespace Fly_Away.DTOs;

public class FlightDto
{
    public int Flight_ID { get; set; }
    public string Status { get; set; } = string.Empty;

    public int Source_ID { get; set; }
    public string SourceName { get; set; } = string.Empty;

    public int Destination_ID { get; set; }
    public string DestinationName { get; set; } = string.Empty;

    public int Airline_ID { get; set; }
    public string AirlineName { get; set; } = string.Empty;

    public DateOnly DateDeparture { get; set; }
    public TimeOnly TimeDeparture { get; set; }
    public DateOnly DateArrival { get; set; }
    public TimeOnly TimeArrival { get; set; }
}
