﻿using System;
using Microsoft.AspNetCore.Mvc;
using Flights.ReadModels;
using Flights.Domain.Entities;
using Flights.DTOs;
using Flights.Domain.Errors;
using Flights.Data;

namespace Flights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(500)]
    [ProducesResponseType(400)]
    public class FlightController : ControllerBase
    {
        private readonly ILogger<FlightController> _logger;

        private readonly Entities _entities;

        public FlightController(ILogger<FlightController> logger, Entities entities)
        {
            _logger = logger;
            _entities = entities;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FlightRm>), 200)]
        public IEnumerable<FlightRm> Search()
        {
            var flightRmList = _entities.Flights.Select(flight => new FlightRm(
                flight.Id,
                flight.Airline,
                flight.Price,
                new TimePlaceRm(flight.Departure.Place.ToString(), flight.Departure.Time),
                new TimePlaceRm(flight.Arrival.Place.ToString(), flight.Arrival.Time),
                flight.RemainingSeats
                ));

            return flightRmList;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(FlightRm), 200)]
        public ActionResult<FlightRm> Find(Guid id)
        {
            var flight = _entities.Flights.SingleOrDefault(f => f.Id == id);

            if (flight == null)
                return NotFound();

            var readModelConversion = new FlightRm(
                flight.Id,
                flight.Airline,
                flight.Price,
                new TimePlaceRm(flight.Departure.Place.ToString(), flight.Departure.Time),
                new TimePlaceRm(flight.Arrival.Place.ToString(), flight.Arrival.Time),
                flight.RemainingSeats
                );

            return Ok(readModelConversion);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200)]
        public IActionResult Book(BookDto dto)
        {
            System.Diagnostics.Debug.WriteLine($"Booking a new flight {dto.FlightId}");

            var flight = _entities.Flights.SingleOrDefault(f => f.Id == dto.FlightId);

            if (flight == null)
                return NotFound();

            var error = flight.MakeBooking(dto.PassengerEmail, dto.NumOfSeats);

            if (error is OverbookError)
            {
                return Conflict(new { message = "Not enough seats available" });
            }

            _entities.SaveChanges();

            return CreatedAtAction(nameof(Find), new { id = dto.FlightId });

        }


    }
}