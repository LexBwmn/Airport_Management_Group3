using Fly_Away.Models;

namespace Fly_Away.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        // If already seeded, do nothing
        if (db.Airlines.Any() || db.Airports.Any())
            return;

        // Airlines
        var airlines = new List<Airline>
        {
            new() { Name = "Air Canada" },
            new() { Name = "WestJet" },
            new() { Name = "Delta Air Lines" },
            new() { Name = "United Airlines" },
            new() { Name = "American Airlines" },
            new() { Name = "Lufthansa" },
            new() { Name = "British Airways" }
        };
        db.Airlines.AddRange(airlines);

        // Airports
        var airports = new List<Airport>
        {
            new() { Name = "Toronto Pearson International Airport" },   // 1
            new() { Name = "Vancouver International Airport" },         // 2
            new() { Name = "Montréal–Trudeau International Airport" },  // 3
            new() { Name = "Calgary International Airport" },           // 4
            new() { Name = "Ottawa Macdonald–Cartier International Airport" }, // 5
            new() { Name = "John F. Kennedy International Airport" },   // 6
            new() { Name = "Los Angeles International Airport" },       // 7
            new() { Name = "Chicago O'Hare International Airport" },    // 8
            new() { Name = "London Heathrow Airport" },                 // 9
            new() { Name = "Frankfurt Airport" },                       // 10
            new() { Name = "Tokyo Haneda Airport" },                    // 11
            new() { Name = "Sydney Kingsford Smith Airport" }           // 12
        };
        db.Airports.AddRange(airports);

        // Flight Classes
        var classes = new List<FlightClass>
        {
            new() { BaggageSize = "Small", SeatType = "Economy" },
            new() { BaggageSize = "Medium", SeatType = "Business" },
            new() { BaggageSize = "Large", SeatType = "First" }
        };
        db.FlightClasses.AddRange(classes);

        db.SaveChanges(); // must save to get IDs

        // Flights (simple deterministic list)
        var rndFlights = new List<Flight>
        {
            new()
            {
                Source_ID = db.Airports.First(a => a.Name.StartsWith("Toronto")).Airport_ID,
                Destination_ID = db.Airports.First(a => a.Name.StartsWith("John F. Kennedy")).Airport_ID,
                Airline_ID = db.Airlines.First(a => a.Name == "Air Canada").Airline_ID,
                Status = "Scheduled",
                DateDeparture = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeDeparture = new TimeOnly(10, 30),
                DateArrival = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeArrival = new TimeOnly(12, 00)
            },
            new()
            {
                Source_ID = db.Airports.First(a => a.Name.StartsWith("Toronto")).Airport_ID,
                Destination_ID = db.Airports.First(a => a.Name.StartsWith("Vancouver")).Airport_ID,
                Airline_ID = db.Airlines.First(a => a.Name == "WestJet").Airline_ID,
                Status = "Scheduled",
                DateDeparture = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                TimeDeparture = new TimeOnly(14, 15),
                DateArrival = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                TimeArrival = new TimeOnly(18, 05)
            }
        };

        // add extra flights (simple pattern)
        var airportIds = db.Airports.Select(a => a.Airport_ID).ToList();
        var airlineIds = db.Airlines.Select(a => a.Airline_ID).ToList();

        int day = 3;
        for (int i = 0; i < 8; i++)
        {
            int source = airportIds[i % airportIds.Count];
            int dest = airportIds[(i + 3) % airportIds.Count];
            if (source == dest) dest = airportIds[(i + 4) % airportIds.Count];

            rndFlights.Add(new Flight
            {
                Source_ID = source,
                Destination_ID = dest,
                Airline_ID = airlineIds[i % airlineIds.Count],
                Status = "Scheduled",
                DateDeparture = DateOnly.FromDateTime(DateTime.Today.AddDays(day)),
                TimeDeparture = new TimeOnly(9 + (i % 5), 0),
                DateArrival = DateOnly.FromDateTime(DateTime.Today.AddDays(day)),
                TimeArrival = new TimeOnly(11 + (i % 5), 0)
            });

            day++;
        }

        db.Flights.AddRange(rndFlights);
        db.SaveChanges();

        // Seats + Gates for each flight
        foreach (var flight in db.Flights.ToList())
        {
            // Seats: 30
            for (int s = 1; s <= 30; s++)
            {
                string seatType = (s % 3) switch
                {
                    0 => "Window",
                    1 => "Middle",
                    _ => "Aisle"
                };

                db.Seats.Add(new Seat
                {
                    SeatNum = s,
                    SeatType = seatType,
                    Flight_ID = flight.Flight_ID
                });
            }

            // Gate: 1 per flight at source airport
            db.Gates.Add(new Gate
            {
                Status = "Open",
                Airport_ID = flight.Source_ID,
                Flight_ID = flight.Flight_ID
            });
        }

        db.SaveChanges();
    }
}
