// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

namespace BookMate.Core.Api.Models.Restaurants
{
    public class RestaurantSearchCriteria
    {
        public string Name { get; set; }
        public RestaurantCoordinates Coordinates { get; set; }
    }
}
