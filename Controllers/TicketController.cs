using flight_ticket.Data;
using flight_ticket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace flight_ticket.Controllers
{
    [Route("api/tickets")]
[ApiController]
[Authorize(Roles = "Customer")]
public class TicketController : ControllerBase
{
    private readonly TicketContext _ticketContext;
    private readonly FlightContext _flightContext;
    private readonly UserManager<IdentityUser> _userManager;

    public TicketController(TicketContext ticketContext, FlightContext flightContext, UserManager<IdentityUser> userManager)
    {
        _ticketContext = ticketContext;
        _flightContext = flightContext;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetTickets()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        var tickets = await _ticketContext.Tickets
            .Where(t => t.PassengerEmail == user.Email)
            .ToListAsync();

        return Ok(tickets);
    }

    [HttpPost]
    public async Task<IActionResult> BookTicket([FromBody] Ticket ticket)
    {
        var flight = await _flightContext.Flights.FindAsync(ticket.FlightId);
        if (flight != null || flight.AvailableSeats <= 0)
        {
            return BadRequest("Flight not available.");
        }

        ticket.PassengerEmail = (await _userManager.FindByNameAsync(User.Identity.Name)).Email;
        _ticketContext.Tickets.Add(ticket);
        flight.AvailableSeats--;
        await _ticketContext.SaveChangesAsync();
        await _flightContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTickets), new { id = ticket.Id }, ticket);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicket(int id)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        var ticket = await _ticketContext.Tickets.FindAsync(id);

        if (ticket == null || ticket.PassengerEmail != user.Email)
        {
            return NotFound("there is no ticket with this id for you");
        }

        return Ok(ticket);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelTicket(int id)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        var ticket = await _ticketContext.Tickets.FindAsync(id);

        if (ticket == null || ticket.PassengerEmail != user.Email)
        {
            return NotFound("there is no ticket with this id");
        }

        ticket.Status = "Cancelled";
        var flight = await _flightContext.Flights.FindAsync(ticket.FlightId);
        flight.AvailableSeats++;
        
        await _ticketContext.SaveChangesAsync();
        await _flightContext.SaveChangesAsync();

        return NoContent();
    }
}

}
