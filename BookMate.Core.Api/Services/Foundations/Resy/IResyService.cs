// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.Resy;
using BookMate.Core.Api.Models.Restaurants;
using BookMate.Core.Api.Models.Resy;

namespace BookMate.Core.Api.Services.Foundations.Resy
{
    public interface IResyService
    {
        ValueTask<ExternalResyProfile> AuthenticateWithCredentialsAsync(string email, string password);
        ValueTask<List<Restaurant>> SearchRestaurantsAsync(RestaurantSearchCriteria restaurantSearchCriteria, string resyToken = "");
        ValueTask<List<ResyRestaurantSlot>> SearchRestaurantAvailabilityAsync(RestaurantReservationCriteria restaurantReservationCriteria, string resyToken = "");
        ValueTask<ResyRestaurantSlotDetails> GetRestaurantSlotDetailsAsync(ResyRestaurantSlotDetailsSearch resyRestaurantSlotDetailsSearch, string toresyTokenken = "");
        ValueTask<ResyRestaurantSlotBooking> BookRestaurantSlotAsync(ResyRestaurantSlotBookingCriteria resyRestaurantSlotBookingCriteria, string resyToken = "");
        //ValueTask<ExternalResyReservationCancellationResult> CancelReservationAsync(string reservationToken, string resyToken);
    }
}
