// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.OpenTable;
using BookMate.Core.Api.Models.Restaurants;

namespace BookMate.Core.Api.Services.Foundations.OpenTable
{
    public interface IOpenTableService
    {
        ValueTask<List<Restaurant>> SearchRestaurantsAsync(RestaurantSearchCriteria restaurantSearchCriteria);
        ValueTask<ExternalOpenTableRestaurantsAvailabilityResult> SearchRestaurantAvailabilityAsync(RestaurantReservationCriteria restaurantReservationCriteria);
        ValueTask<ExternalOpenTableSlotLockBookingResult> BookRestaurantLockSlotAsync(ExternalOpenTableSlotLockBooking externalOpenTableSlotLockBooking);
        ValueTask<ExternalOpenTableBookingResult> BookRestaurantAsyncAsync(ExternalOpenTableBooking externalOpenTableBooking);
    }
}
