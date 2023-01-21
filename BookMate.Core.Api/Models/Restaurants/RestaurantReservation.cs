// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;

namespace BookMate.Core.Api.Models.Restaurants
{
    public class RestaurantReservation
    {
        public RestaurantBookingSystem BookingSystem { get; set; }
        public string RestaurantId { get; set; }
        public string ReservationId { get; set; }
        public string ReservationToken { get; set; }
        public string ConfirmationNumber { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public int PartySize { get; set; }
        public string Environment { get; set; }
    }
}
