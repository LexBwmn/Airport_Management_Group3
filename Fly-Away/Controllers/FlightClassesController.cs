using Fly_Away.Data;
using Fly_Away.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fly_Away.Controllers;

[ApiController]
[Route("api/flightclasses")]
public class FlightClassesController : ControllerBase
{
    private readonly AppDbContext _db;

    public FlightClassesController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/flightclasses
    [HttpGet]
    public async Task<ActionResult<List<FlightClassDto>>> GetAll()
    {
        var list = await _db.FlightClasses
            .AsNoTracking()
            .OrderBy(fc => fc.SeatType)
            .ThenBy(fc => fc.BaggageSize)
            .Select(fc => new FlightClassDto
            {
                FlightClass_ID = fc.FlightClass_ID,
                SeatType = fc.SeatType,
                BaggageSize = fc.BaggageSize
            })
            .ToListAsync();

        return Ok(list);
    }
}
