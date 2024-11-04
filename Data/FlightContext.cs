using flight_ticket.Models;
using Microsoft.EntityFrameworkCore;

namespace flight_ticket.Data
{
    public class FlightContext : DbContext
    {
        public FlightContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Flight> Flights { get; set; }
    }
}
