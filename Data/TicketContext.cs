using flight_ticket.Models;
using Microsoft.EntityFrameworkCore;

namespace flight_ticket.Data
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
