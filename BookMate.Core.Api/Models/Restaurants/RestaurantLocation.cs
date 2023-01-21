// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

namespace BookMate.Core.Api.Models.Restaurants
{
    public class RestaurantLocation
    {
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Neighborhood { get; set; }
        public string Borough { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public RestaurantCoordinates Coordinates { get; set; }
    }
}