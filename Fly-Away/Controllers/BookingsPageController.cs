using Fly_Away.Data;
using Fly_Away.Models;
using Fly_Away.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fly_Away.Controllers;

public class BookingsPageController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public BookingsPageController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    private int? GetAccountId()
    {
        return HttpContext.Session.GetInt32("AccountId");
    }

    // POST: /BookingsPage/Create
    [HttpPost]
    public async Task<IActionResult> Create(int flight_ID, int flightClass_ID)
    {
        var accountId = GetAccountId();
        if (accountId == null) return RedirectToAction("Login", "AuthPage");

        var flight = await _db.Flights.AsNoTracking().FirstOrDefaultAsync(f => f.Flight_ID == flight_ID);
        if (flight == null) return BadRequest("Invalid Flight_ID");

        var fc = await _db.FlightClasses.AsNoTracking().FirstOrDefaultAsync(x => x.FlightClass_ID == flightClass_ID);
        if (fc == null) return BadRequest("Invalid FlightClass_ID");

        var gate = await _db.Gates.AsNoTracking().FirstOrDefaultAsync(g => g.Flight_ID == flight_ID);
        if (gate == null) return BadRequest("No gate available");

        var usedSeatIds = await _db.Tickets
            .AsNoTracking()
            .Where(t => t.Flight_ID == flight_ID)
            .Select(t => t.Seat_ID)
            .ToListAsync();

        var seat = await _db.Seats
            .AsNoTracking()
            .Where(s => s.Flight_ID == flight_ID && !usedSeatIds.Contains(s.Seat_ID))
            .OrderBy(s => s.SeatNum)
            .FirstOrDefaultAsync();

        if (seat == null) return BadRequest("No seats available");

        var ticket = new Ticket
        {
            Barcode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Account_ID = accountId.Value,
            Flight_ID = flight_ID,
            FlightClass_ID = flightClass_ID,
            Seat_ID = seat.Seat_ID,
            Gate_ID = gate.Gate_ID,

            // temporary
            TicketImageUrl = ""
        };

        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync(); // now Ticket_ID exists

        // generate PNG file + get URL
        var url = await TicketImageService.GenerateAsync(_db, ticket.Ticket_ID, _env.WebRootPath);

        // store correct URL
        ticket.TicketImageUrl = url;
        await _db.SaveChangesAsync();

        return RedirectToAction("MyBookings");
    }

    // GET: /BookingsPage/Details/1
    public async Task<IActionResult> Details(int id)
    {
        var accountId = GetAccountId();
        if (accountId == null) return RedirectToAction("Login", "AuthPage");

        var t = await _db.Tickets
            .AsNoTracking()
            .Where(x => x.Ticket_ID == id && x.Account_ID == accountId.Value)
            .Include(x => x.Flight)!.ThenInclude(f => f!.Airline)
            .Include(x => x.Flight)!.ThenInclude(f => f!.Source)
            .Include(x => x.Flight)!.ThenInclude(f => f!.Destination)
            .Include(x => x.FlightClass)
            .Include(x => x.Seat)
            .Include(x => x.Gate)
            .FirstOrDefaultAsync();

        if (t == null) return NotFound();

        return View(t);
    }

    // POST: /BookingsPage/Cancel/6
    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var accountId = GetAccountId();
        if (accountId == null) return RedirectToAction("Login", "AuthPage");

        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Ticket_ID == id && t.Account_ID == accountId.Value);
        if (ticket == null) return NotFound();

        _db.Tickets.Remove(ticket);
        await _db.SaveChangesAsync();

        return RedirectToAction("MyBookings");
    }

    // POST: /BookingsPage/ChangeClass
    [HttpPost]
    public async Task<IActionResult> ChangeClass(int ticketId, int flightClassId)
    {
        var accountId = GetAccountId();
        if (accountId == null) return RedirectToAction("Login", "AuthPage");

        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Ticket_ID == ticketId && t.Account_ID == accountId.Value);
        if (ticket == null) return NotFound();

        var fc = await _db.FlightClasses.AsNoTracking().FirstOrDefaultAsync(x => x.FlightClass_ID == flightClassId);
        if (fc == null) return BadRequest("Invalid FlightClass_ID");

        ticket.FlightClass_ID = flightClassId;
        await _db.SaveChangesAsync();

        return RedirectToAction("Details", new { id = ticketId });
    }


    // GET: /BookingsPage/MyBookings
    public async Task<IActionResult> MyBookings()
    {
        var accountId = GetAccountId();
        if (accountId == null) return RedirectToAction("Login", "AuthPage");

        var tickets = await _db.Tickets
            .AsNoTracking()
            .Where(t => t.Account_ID == accountId.Value)
            .Include(t => t.Flight)!.ThenInclude(f => f!.Airline)
            .Include(t => t.Flight)!.ThenInclude(f => f!.Source)
            .Include(t => t.Flight)!.ThenInclude(f => f!.Destination)
            .Include(t => t.FlightClass)
            .OrderByDescending(t => t.Ticket_ID)
            .ToListAsync();

        return View(tickets);
    }
}
