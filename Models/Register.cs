using flight_ticket.Data;

namespace flight_ticket.Models
{
    public class Register
    {

        public string? Username { get; set; } = string.Empty;

        public string? Password { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        //public UserRole Role { get; set; }
    }
}
