using Flights.Domain.Errors;
using Flights.ReadModels;

namespace Flights.Domain.Entities
{
    public record Flight(
        Guid Id,
        string Airline,
        string Price,
        TimePlace Departure,
        TimePlace Arrival,
        int RemainingSeats
        )
    {
        public IList<Booking> Bookings = new List<Booking>();
        public int RemainingSeats { get; set; } = RemainingSeats;
        public object? MakeBooking(string passengerEmail, byte numOfSeats) {

            var flight = this;

            if (flight.RemainingSeats < numOfSeats)
            {
                return new OverbookError();
            }

            flight.Bookings.Add(
                new Booking(
                    passengerEmail,
                    numOfSeats
            )
            );

            flight.RemainingSeats -= numOfSeats;
            return null;
        }
    }
}
