using System.ComponentModel.DataAnnotations;

namespace Flights.DTOs
{
    public record NewPassengerDto
    (
        [Required][StringLength(100, MinimumLength = 3)] string Email,
        [Required] string FirstName,
        [Required] string LastName,
        [Required] bool Gender
    );
}
