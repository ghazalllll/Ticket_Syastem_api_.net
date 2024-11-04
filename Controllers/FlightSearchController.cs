using flight_ticket.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace flight_ticket.Controllers
{
    [Route("api/flights/search")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class FlightSearchController : ControllerBase
    {
        private readonly FlightContext _flightContext;

        public FlightSearchController(FlightContext flightContext)
        {
            _flightContext = flightContext;
        }

        [HttpGet]
        public async Task<IActionResult> SearchFlights([FromQuery] string departureCity, [FromQuery] string arrivalCity, [FromQuery] DateTime? departureDate)
        {
            var flights = await _flightContext.Flights
                .Where(f =>
                    (string.IsNullOrEmpty(departureCity) || f.DepartureCity == departureCity) &&
                    (string.IsNullOrEmpty(arrivalCity) || f.ArrivalCity == arrivalCity) &&
                    (!departureDate.HasValue || f.DepartureTime.Date == departureDate.Value.Date))
                .ToListAsync();

            return Ok(flights);
        }
    }

}
