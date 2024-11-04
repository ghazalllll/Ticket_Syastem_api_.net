using flight_ticket.Data;
using flight_ticket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace flight_ticket.Controllers
{
    [Route("api/flights")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FlightController : ControllerBase
    {
        private readonly FlightContext _flightcontext;

        public FlightController(FlightContext flightcontext)
        {
            _flightcontext = flightcontext;
        }

        [HttpGet]
        public async Task<IActionResult> GetFlights()
        {
            var flights = await _flightcontext.Flights.ToListAsync();
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlight(int id)
        {
            var flight = await _flightcontext.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound("Your desired flight was not found");
            }
            return Ok(flight);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlight([FromBody] Flight flight)
        {
            if (flight == null)
            {
                return BadRequest("Please complete the flight information");
            }

            _flightcontext.Flights.Add(flight);
            await _flightcontext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFlight), new { id = flight.Id }, flight);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlight(int id, [FromBody] Flight flight)
        {
            if (id != flight.Id)
            {
                return BadRequest();
            }

            _flightcontext.Entry(flight).State = EntityState.Modified;

            try
            {
                await _flightcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
                {
                    return NotFound("This flight does not exist");
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var flight = await _flightcontext.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            _flightcontext.Flights.Remove(flight);
            await _flightcontext.SaveChangesAsync();
            return NoContent();
        }

        private bool FlightExists(int id)
        {
            return _flightcontext.Flights.Any(i => i.Id == id);
        }
    }

}
