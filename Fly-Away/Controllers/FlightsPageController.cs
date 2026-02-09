using Fly_Away.Data;
using Fly_Away.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fly_Away.Controllers;

public class FlightsPageController : Controller
{
    private readonly AppDbContext _db;

    public FlightsPageController(AppDbContext db)
    {
        _db = db;
    }

    // URL: /FlightsPage/Details/1
    public async Task<IActionResult> Details(int id)
    {
        var flight = await _db.Flights
            .AsNoTracking()
            .Include(f => f.Airline)
            .Include(f => f.Source)
            .Include(f => f.Destination)
            .FirstOrDefaultAsync(f => f.Flight_ID == id);

        if (flight == null) return NotFound();

        var classes = await _db.FlightClasses
            .AsNoTracking()
            .Select(fc => new FlightClassItem
            {
                FlightClass_ID = fc.FlightClass_ID,
                SeatType = fc.SeatType,
                BaggageSize = fc.BaggageSize
            })
            .ToListAsync();

        var vm = new FlightDetailsViewModel
        {
            Flight_ID = flight.Flight_ID,
            AirlineName = flight.Airline!.Name,
            SourceName = flight.Source!.Name,
            DestinationName = flight.Destination!.Name,
            DateDeparture = flight.DateDeparture,
            TimeDeparture = flight.TimeDeparture,
            DateArrival = flight.DateArrival,
            TimeArrival = flight.TimeArrival,
            Classes = classes
        };

        return View(vm);
    }
}
