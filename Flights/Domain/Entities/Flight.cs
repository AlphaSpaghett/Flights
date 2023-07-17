using Flights.Domain.Entities;
using Flights.Domain.Errors;
using Flights.ReadModels;

namespace Flights.Domain.Entities
{
    public class Flight
    {
        public Guid Id { get; set; }
        public string Airline { get; set;}
        public string Price { get; set;}
        public TimePlace Departure { get; set;}
        public TimePlace Arrival { get; set;}
        public int RemainingSeats { get; set;}


        public IList<Booking> Bookings = new List<Booking>();

        public Flight() 
        {

        }

        public Flight(
            Guid id,
            string airline,
            string price,
            TimePlace departure,
            TimePlace arrival,
            int remainingSeats
        ) {
            Id = id;
            Airline = airline;
            Price = price;
            Departure = departure;
            Arrival = arrival;
            RemainingSeats = remainingSeats;
        }


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



