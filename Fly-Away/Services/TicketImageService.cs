using Fly_Away.Data;
using Microsoft.EntityFrameworkCore;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Fly_Away.Services;

public static class TicketImageService
{
    public static async Task<string> GenerateAsync(AppDbContext db, int ticketId, string webRootPath)
    {
        // Load ticket with details (for writing on image)
        var t = await db.Tickets
            .AsNoTracking()
            .Include(x => x.Flight).ThenInclude(f => f!.Airline)
            .Include(x => x.Flight).ThenInclude(f => f!.Source)
            .Include(x => x.Flight).ThenInclude(f => f!.Destination)
            .Include(x => x.Seat)
            .Include(x => x.Gate)
            .Include(x => x.FlightClass)
            .FirstOrDefaultAsync(x => x.Ticket_ID == ticketId);

        if (t == null) throw new Exception("Ticket not found");

        // Ensure folder exists: wwwroot/tickets
        var ticketsDir = Path.Combine(webRootPath, "tickets");
        Directory.CreateDirectory(ticketsDir);

        var fileName = $"ticket_{ticketId}.png";
        var filePath = Path.Combine(ticketsDir, fileName);

        // Create image
        using var img = new Image<Rgba32>(900, 500);
        img.Mutate(ctx =>
        {
            ctx.Fill(Color.White);

            // Header bar
            ctx.Fill(Color.LightGray, new Rectangle(0, 0, 900, 60));
            ctx.Draw(Color.Gray, 2, new Rectangle(10, 10, 880, 480));

            // Fonts (simple default)
            var titleFont = SystemFonts.CreateFont("Arial", 28, FontStyle.Bold);
            var font = SystemFonts.CreateFont("Arial", 18, FontStyle.Regular);
            var bold = SystemFonts.CreateFont("Arial", 18, FontStyle.Bold);

            // Title
            ctx.DrawText("Fly-Away Ticket", titleFont, Color.Black, new PointF(20, 15));

            float y = 90;

            void Line(string label, string value)
            {
                ctx.DrawText(label, bold, Color.Black, new PointF(30, y));
                ctx.DrawText(value, font, Color.Black, new PointF(250, y));
                y += 35;
            }

            Line("Ticket ID:", t.Ticket_ID.ToString());
            Line("Barcode:", t.Barcode.ToString());
            Line("Airline:", t.Flight!.Airline!.Name);
            Line("From:", t.Flight!.Source!.Name);
            Line("To:", t.Flight!.Destination!.Name);
            Line("Departure:", $"{t.Flight!.DateDeparture} {t.Flight!.TimeDeparture}");
            Line("Arrival:", $"{t.Flight!.DateArrival} {t.Flight!.TimeArrival}");
            Line("Class:", $"{t.FlightClass!.SeatType} / {t.FlightClass!.BaggageSize}");
            Line("Seat:", $"{t.Seat!.SeatNum} ({t.Seat!.SeatType})");
            Line("Gate:", $"{t.Gate!.Gate_ID} ({t.Gate!.Status})");

            // Simple barcode box (visual only)
            ctx.Fill(Color.Black, new Rectangle(650, 380, 220, 10));
            ctx.Fill(Color.Black, new Rectangle(650, 400, 180, 10));
            ctx.Fill(Color.Black, new Rectangle(650, 420, 200, 10));
        });

        // Save PNG
        await img.SaveAsync(filePath, new PngEncoder());

        // Return URL used by UI
        return $"/tickets/{fileName}";
    }
}
