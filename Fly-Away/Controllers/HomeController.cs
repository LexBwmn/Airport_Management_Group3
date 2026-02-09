using System.Diagnostics;
using Fly_Away.Data;
using Fly_Away.Models;
using Fly_Away.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fly_Away.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // HOME PAGE = show flights
        public async Task<IActionResult> Index()
        {
            var flights = await _db.Flights
                .AsNoTracking()
                .Include(f => f.Airline)
                .Include(f => f.Source)
                .Include(f => f.Destination)
                .Select(f => new FlightViewModel
                {
                    Flight_ID = f.Flight_ID,
                    AirlineName = f.Airline!.Name,
                    SourceName = f.Source!.Name,
                    DestinationName = f.Destination!.Name,
                    DateDeparture = f.DateDeparture,
                    TimeDeparture = f.TimeDeparture
                })
                .ToListAsync();

            return View(flights);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
