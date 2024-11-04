using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace flight_ticket.Data
{
    public class Context : IdentityDbContext<IdentityUser>
    {
        public Context(DbContextOptions options) : base(options)
        {
        }
    }
}
