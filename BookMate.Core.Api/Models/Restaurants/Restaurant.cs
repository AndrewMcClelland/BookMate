// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;

namespace BookMate.Core.Api.Models.Restaurants
{
    public class Restaurant
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public RestaurantLocation Location { get; set; }
        public RestaurantContactInformation ContactInformation { get; set; }
        public RestaurantRating Rating { get; set; }
        public RestaurantPriceRange PriceRange { get; set; }
        public List<string> Cuisines { get; set; }
        public RestaurantBookingSystem BookingSystem { get; set; }
    }
}
