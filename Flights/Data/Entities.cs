using Flights.Domain.Entities;
using System;

namespace Flights.Data
{
    public class Entities
    {
        public IList<Passenger> Passengers = new List<Passenger>();

        public List<Flight> Flights = new List<Flight>();

    }
}
