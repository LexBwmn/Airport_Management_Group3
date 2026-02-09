using Fly_Away.Data;
using Fly_Away.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fly_Away.Controllers;

[ApiController]
[Route("api/flights")]
public class FlightsController : ControllerBase
{
    private readonly AppDbContext _db;

    public FlightsController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/flights
    [HttpGet]
    public async Task<ActionResult<List<FlightDto>>> GetAll()
    {
        var flights = await _db.Flights
            .AsNoTracking()
            .Include(f => f.Source)
            .Include(f => f.Destination)
            .Include(f => f.Airline)
            .OrderBy(f => f.DateDeparture)
            .ThenBy(f => f.TimeDeparture)
            .Select(f => new FlightDto
            {
                Flight_ID = f.Flight_ID,
                Status = f.Status,

                Source_ID = f.Source_ID,
                SourceName = f.Source != null ? f.Source.Name : "",

                Destination_ID = f.Destination_ID,
                DestinationName = f.Destination != null ? f.Destination.Name : "",

                Airline_ID = f.Airline_ID,
                AirlineName = f.Airline != null ? f.Airline.Name : "",

                DateDeparture = f.DateDeparture,
                TimeDeparture = f.TimeDeparture,
                DateArrival = f.DateArrival,
                TimeArrival = f.TimeArrival
            })
            .ToListAsync();

        return Ok(flights);
    }

    // GET /api/flights/search?sourceId=1&destinationId=2&date=2026-02-10
    [HttpGet("search")]
    public async Task<ActionResult<List<FlightDto>>> Search(
        [FromQuery] int? sourceId,
        [FromQuery] int? destinationId,
        [FromQuery] DateOnly? date)
    {
        var q = _db.Flights
            .AsNoTracking()
            .Include(f => f.Source)
            .Include(f => f.Destination)
            .Include(f => f.Airline)
            .AsQueryable();

        if (sourceId.HasValue)
            q = q.Where(f => f.Source_ID == sourceId.Value);

        if (destinationId.HasValue)
            q = q.Where(f => f.Destination_ID == destinationId.Value);

        if (date.HasValue)
            q = q.Where(f => f.DateDeparture == date.Value);

        var flights = await q
            .OrderBy(f => f.DateDeparture)
            .ThenBy(f => f.TimeDeparture)
            .Select(f => new FlightDto
            {
                Flight_ID = f.Flight_ID,
                Status = f.Status,

                Source_ID = f.Source_ID,
                SourceName = f.Source != null ? f.Source.Name : "",

                Destination_ID = f.Destination_ID,
                DestinationName = f.Destination != null ? f.Destination.Name : "",

                Airline_ID = f.Airline_ID,
                AirlineName = f.Airline != null ? f.Airline.Name : "",

                DateDeparture = f.DateDeparture,
                TimeDeparture = f.TimeDeparture,
                DateArrival = f.DateArrival,
                TimeArrival = f.TimeArrival
            })
            .ToListAsync();

        return Ok(flights);
    }
}
