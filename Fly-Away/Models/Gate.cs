namespace Fly_Away.Models;

public class Gate
{
    public int Gate_ID { get; set; }
    public string Status { get; set; } = "Open"; // Open/Closed/Boarding

    public int Airport_ID { get; set; }
    public int Flight_ID { get; set; }

    public Airport? Airport { get; set; }
    public Flight? Flight { get; set; }
}
