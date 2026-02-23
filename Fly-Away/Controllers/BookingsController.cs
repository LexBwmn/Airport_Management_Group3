using Fly_Away.Data;
using Fly_Away.DTOs;
using Fly_Away.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fly_Away.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public BookingsController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    private int? GetAccountId()
    {
        return HttpContext.Session.GetInt32("AccountId");
    }

    // GET /api/bookings
    [HttpGet]
    public async Task<ActionResult<List<TicketDto>>> GetMyBookings()
    {
        var accountId = GetAccountId();
        if (accountId == null) return Unauthorized(new { message = "Not logged in" });

        var list = await _db.Tickets
            .AsNoTracking()
            .Where(t => t.Account_ID == accountId.Value)
            .Include(t => t.Flight)!.ThenInclude(f => f!.Airline)
            .Include(t => t.Flight)!.ThenInclude(f => f!.Source)
            .Include(t => t.Flight)!.ThenInclude(f => f!.Destination)
            .Include(t => t.Seat)
            .Include(t => t.Gate)
            .Include(t => t.FlightClass)
            .OrderByDescending(t => t.Ticket_ID)
            .Select(t => new TicketDto
            {
                Ticket_ID = t.Ticket_ID,
                Barcode = t.Barcode,

                Flight_ID = t.Flight_ID,
                AirlineName = t.Flight!.Airline!.Name,

                SourceName = t.Flight!.Source!.Name,
                DestinationName = t.Flight!.Destination!.Name,

                DateDeparture = t.Flight!.DateDeparture,
                TimeDeparture = t.Flight!.TimeDeparture,
                DateArrival = t.Flight!.DateArrival,
                TimeArrival = t.Flight!.TimeArrival,

                FlightClass = t.FlightClass!.SeatType,
                BaggageSize = t.FlightClass!.BaggageSize,

                SeatNum = t.Seat!.SeatNum,
                SeatType = t.Seat!.SeatType,

                GateStatus = t.Gate!.Status,
                TicketImageUrl = t.TicketImageUrl ?? ""
            })
            .ToListAsync();

        return Ok(list);
    }

    // GET /api/bookings/{ticketId}
    [HttpGet("{ticketId:int}")]
    public async Task<ActionResult<TicketDto>> GetOne(int ticketId)
    {
        var accountId = GetAccountId();
        if (accountId == null) return Unauthorized(new { message = "Not logged in" });

        var t = await _db.Tickets
            .AsNoTracking()
            .Where(x => x.Ticket_ID == ticketId && x.Account_ID == accountId.Value)
            .Include(x => x.Flight)!.ThenInclude(f => f!.Airline)
            .Include(x => x.Flight)!.ThenInclude(f => f!.Source)
            .Include(x => x.Flight)!.ThenInclude(f => f!.Destination)
            .Include(x => x.Seat)
            .Include(x => x.Gate)
            .Include(x => x.FlightClass)
            .FirstOrDefaultAsync();

        if (t == null) return NotFound(new { message = "Ticket not found" });

        return Ok(new TicketDto
        {
            Ticket_ID = t.Ticket_ID,
            Barcode = t.Barcode,

            Flight_ID = t.Flight_ID,
            AirlineName = t.Flight!.Airline!.Name,

            SourceName = t.Flight!.Source!.Name,
            DestinationName = t.Flight!.Destination!.Name,

            DateDeparture = t.Flight!.DateDeparture,
            TimeDeparture = t.Flight!.TimeDeparture,
            DateArrival = t.Flight!.DateArrival,
            TimeArrival = t.Flight!.TimeArrival,

            FlightClass = t.FlightClass!.SeatType,
            BaggageSize = t.FlightClass!.BaggageSize,

            SeatNum = t.Seat!.SeatNum,
            SeatType = t.Seat!.SeatType,

            GateStatus = t.Gate!.Status,
            TicketImageUrl = t.TicketImageUrl ?? ""
        });
    }

    // POST /api/bookings
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateBookingRequest req)
    {
        var accountId = GetAccountId();
        if (accountId == null) return Unauthorized(new { message = "Not logged in" });

        var flight = await _db.Flights.AsNoTracking().FirstOrDefaultAsync(f => f.Flight_ID == req.Flight_ID);
        if (flight == null) return BadRequest(new { message = "Invalid Flight_ID" });

        var flightClass = await _db.FlightClasses.AsNoTracking().FirstOrDefaultAsync(fc => fc.FlightClass_ID == req.FlightClass_ID);
        if (flightClass == null) return BadRequest(new { message = "Invalid FlightClass_ID" });

        var gate = await _db.Gates.AsNoTracking().FirstOrDefaultAsync(g => g.Flight_ID == req.Flight_ID);
        if (gate == null) return BadRequest(new { message = "No gate available for this flight" });

        var usedSeatIds = await _db.Tickets
            .AsNoTracking()
            .Where(t => t.Flight_ID == req.Flight_ID)
            .Select(t => t.Seat_ID)
            .ToListAsync();

        var seat = await _db.Seats
            .AsNoTracking()
            .Where(s => s.Flight_ID == req.Flight_ID && !usedSeatIds.Contains(s.Seat_ID))
            .OrderBy(s => s.SeatNum)
            .FirstOrDefaultAsync();

        if (seat == null) return BadRequest(new { message = "No seats available for this flight" });

        var ticket = new Ticket
        {
            Barcode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Account_ID = accountId.Value,
            FlightClass_ID = req.FlightClass_ID,
            Seat_ID = seat.Seat_ID,
            Gate_ID = gate.Gate_ID,
            Flight_ID = req.Flight_ID,

            PassengerName = req.PassengerName,
            Email = req.Email,
            PhoneNumber = req.PhoneNumber,
            PassportNumber = req.PassportNumber,

            TicketImageUrl = $"/tickets/ticket_{0}.png"
        };

        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync();

        ticket.TicketImageUrl = $"/tickets/ticket_{ticket.Ticket_ID}.png";
        await _db.SaveChangesAsync();

        return Created($"/api/bookings/{ticket.Ticket_ID}", new
        {
            message = "Booking created",
            ticketId = ticket.Ticket_ID,
            ticketImageUrl = ticket.TicketImageUrl
        });
    }

    // PUT /api/bookings/{ticketId}
    [HttpPut("{ticketId:int}")]
    public async Task<ActionResult> Put(int ticketId, [FromBody] UpdateBookingRequest req)
    {
        var accountId = GetAccountId();
        if (accountId == null) return Unauthorized(new { message = "Not logged in" });

        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Ticket_ID == ticketId && t.Account_ID == accountId.Value);
        if (ticket == null) return NotFound(new { message = "Ticket not found" });

        var fc = await _db.FlightClasses.AsNoTracking().FirstOrDefaultAsync(x => x.FlightClass_ID == req.FlightClass_ID);
        if (fc == null) return BadRequest(new { message = "Invalid FlightClass_ID" });

        ticket.FlightClass_ID = req.FlightClass_ID;
        ticket.PassengerName = req.PassengerName;
        ticket.Email = req.Email;
        ticket.PhoneNumber = req.PhoneNumber;
        ticket.PassportNumber = req.PassportNumber;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Booking updated (PUT)", ticketId = ticket.Ticket_ID });
    }

    // PATCH /api/bookings/{ticketId}
    [HttpPatch("{ticketId:int}")]
    public async Task<ActionResult> Patch(int ticketId, [FromBody] PatchBookingRequest req)
    {
        var accountId = GetAccountId();
        if (accountId == null) return Unauthorized(new { message = "Not logged in" });

        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Ticket_ID == ticketId && t.Account_ID == accountId.Value);
        if (ticket == null) return NotFound(new { message = "Ticket not found" });

        if (req.FlightClass_ID.HasValue)
        {
            var fc = await _db.FlightClasses.AsNoTracking().FirstOrDefaultAsync(x => x.FlightClass_ID == req.FlightClass_ID.Value);
            if (fc == null) return BadRequest(new { message = "Invalid FlightClass_ID" });
            ticket.FlightClass_ID = req.FlightClass_ID.Value;
        }

        if (req.PassengerName != null) ticket.PassengerName = req.PassengerName;
        if (req.Email != null) ticket.Email = req.Email;
        if (req.PhoneNumber != null) ticket.PhoneNumber = req.PhoneNumber;
        if (req.PassportNumber != null) ticket.PassportNumber = req.PassportNumber;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Booking updated (PATCH)", ticketId = ticket.Ticket_ID });
    }

    // DELETE /api/bookings/{ticketId}
    [HttpDelete("{ticketId:int}")]
    public async Task<ActionResult> Delete(int ticketId)
    {
        var accountId = GetAccountId();
        if (accountId == null) return Unauthorized(new { message = "Not logged in" });

        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Ticket_ID == ticketId && t.Account_ID == accountId.Value);
        if (ticket == null) return NotFound(new { message = "Ticket not found" });

        if (!string.IsNullOrWhiteSpace(ticket.TicketImageUrl))
        {
            var relative = ticket.TicketImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
            var fullPath = Path.Combine(_env.WebRootPath, relative);

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }

        _db.Tickets.Remove(ticket);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Booking deleted", ticketId });
    }

    // OPTIONS /api/bookings
    [HttpOptions]
    public IActionResult Options()
    {
        Response.Headers.Allow = "GET,POST,PUT,PATCH,DELETE,OPTIONS";
        return Ok(new { message = "Allowed methods returned in Allow header" });
    }
}