using flight_ticket.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace flight_ticket.Controllers
{
    [Route("api/admin/tickets")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TicketManagementByAdminController : ControllerBase
    {
        private readonly TicketContext _ticketContext;

        public TicketManagementByAdminController(TicketContext ticketContext)
        {
            _ticketContext = ticketContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = _ticketContext.Tickets.ToListAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var ticket = _ticketContext.Tickets.FindAsync(id);
            if (ticket != null)
            {
                return Ok(ticket);
            }
            return BadRequest();

        }
    }
}
