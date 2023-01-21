// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.Restaurants;

namespace BookMate.Core.Api.Services.Coordinations.Restaurants
{
    public interface IRestaurantCoordinationService
    {
        ValueTask<List<Restaurant>> SearchRestaurantsAsync(RestaurantSearchCriteria restaurantSearchCriteria);
        ValueTask<RestaurantReservation> ReserveRestaurantAsync(RestaurantReservationCriteria restaurantReservationCriteria);
    }
}
