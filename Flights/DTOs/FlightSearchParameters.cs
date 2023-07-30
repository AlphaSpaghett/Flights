using System.ComponentModel;

namespace Flights.DTOs
{
    public record FlightSearchParameters(

        [DefaultValue("12/25/2022 10:30:00 AM")]
        DateTime? FromDate,
        [DefaultValue("12/26/2022 10:30:00 AM")]
        DateTime? ToDate,
        [DefaultValue("Los Angeles")]
        string? Departure,
        [DefaultValue("Berlin")]
        string? Destination,
        [DefaultValue(1)]
        int? NumOfPassengers
    );
}
