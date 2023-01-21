// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.Restaurants;

namespace BookMate.Core.Api.Services.Orchestrations.OpenTable
{
    public interface IOpenTableOrchestrationService
    {
        ValueTask<List<Restaurant>> SearchRestaurantsAsync(RestaurantSearchCriteria restaurantSearchCriteria);
        ValueTask<RestaurantReservation> ReserveRestaurantAsync(RestaurantReservationCriteria restaurantReservationCriteria);
    }
}
