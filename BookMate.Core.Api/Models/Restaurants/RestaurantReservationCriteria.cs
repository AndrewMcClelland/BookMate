// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;

namespace BookMate.Core.Api.Models.Restaurants
{
    public class RestaurantReservationCriteria
    {
        public RestaurantBookingSystem BookingSystem { get; set; }
        public string RestaurantId { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public int PartySize { get; set; }
        public RestaurantCoordinates RestaurantCoordinates { get; set; }
    }
}
