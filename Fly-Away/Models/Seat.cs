namespace Fly_Away.Models;

public class Seat
{
    public int Seat_ID { get; set; }
    public int SeatNum { get; set; }
    public string SeatType { get; set; } = string.Empty; // Window/Middle/Aisle

    public int Flight_ID { get; set; }
    public Flight? Flight { get; set; }
}
