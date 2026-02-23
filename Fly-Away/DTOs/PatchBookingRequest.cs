namespace Fly_Away.DTOs;

public class PatchBookingRequest
{
    public int? FlightClass_ID { get; set; }

    public string? PassengerName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PassportNumber { get; set; }
}