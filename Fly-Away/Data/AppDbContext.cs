using Fly_Away.Models;
using Microsoft.EntityFrameworkCore;

namespace Fly_Away.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Airline> Airlines => Set<Airline>();
    public DbSet<Airport> Airports => Set<Airport>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<FlightClass> FlightClasses => Set<FlightClass>();
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Gate> Gates => Set<Gate>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Table names + primary keys
        modelBuilder.Entity<Airline>().ToTable("Airline").HasKey(x => x.Airline_ID);
        modelBuilder.Entity<Airport>().ToTable("Airport").HasKey(x => x.Airport_ID);
        modelBuilder.Entity<Account>().ToTable("Account").HasKey(x => x.Account_ID);
        modelBuilder.Entity<FlightClass>().ToTable("FlightClass").HasKey(x => x.FlightClass_ID);
        modelBuilder.Entity<Flight>().ToTable("Flight").HasKey(x => x.Flight_ID);
        modelBuilder.Entity<Seat>().ToTable("Seat").HasKey(x => x.Seat_ID);
        modelBuilder.Entity<Gate>().ToTable("Gate").HasKey(x => x.Gate_ID);
        modelBuilder.Entity<Ticket>().ToTable("Ticket").HasKey(x => x.Ticket_ID);

        // Flight relationships
        modelBuilder.Entity<Flight>()
            .HasOne(f => f.Destination)
            .WithMany()
            .HasForeignKey(f => f.Destination_ID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Flight>()
            .HasOne(f => f.Source)
            .WithMany()
            .HasForeignKey(f => f.Source_ID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Flight>()
            .HasOne(f => f.Airline)
            .WithMany()
            .HasForeignKey(f => f.Airline_ID)
            .OnDelete(DeleteBehavior.Restrict);

        // Seat
        modelBuilder.Entity<Seat>()
            .HasOne(s => s.Flight)
            .WithMany(f => f.Seats)
            .HasForeignKey(s => s.Flight_ID)
            .OnDelete(DeleteBehavior.Restrict);

        // Gate
        modelBuilder.Entity<Gate>()
            .HasOne(g => g.Airport)
            .WithMany()
            .HasForeignKey(g => g.Airport_ID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Gate>()
            .HasOne(g => g.Flight)
            .WithMany(f => f.Gates)
            .HasForeignKey(g => g.Flight_ID)
            .OnDelete(DeleteBehavior.Restrict);

        // Ticket 
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Account)
            .WithMany()
            .HasForeignKey(t => t.Account_ID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.FlightClass)
            .WithMany()
            .HasForeignKey(t => t.FlightClass_ID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Seat)
            .WithMany()
            .HasForeignKey(t => t.Seat_ID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Gate)
            .WithMany()
            .HasForeignKey(t => t.Gate_ID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Flight)
            .WithMany(f => f.Tickets)
            .HasForeignKey(t => t.Flight_ID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
