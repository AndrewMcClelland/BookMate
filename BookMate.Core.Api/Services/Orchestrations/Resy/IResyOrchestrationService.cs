// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.Resy;
using BookMate.Core.Api.Models.Restaurants;

namespace BookMate.Core.Api.Services.Orchestrations.Resy
{
    public interface IResyOrchestrationService
    {
        ValueTask<ExternalResyProfile> AuthenticateWithCredentialsAsync(string email, string password);
        ValueTask<List<Restaurant>> SearchRestaurantsAsync(RestaurantSearchCriteria restaurantSearchCriteria);
        //ValueTask<ResyVenueSlotsSearchResult> FindVenueSlotsAsync(VenueSlotsSearch venueSlotsSearch, string resyToken = "");
        //ValueTask<ResyVenueSlotBookingResult> ReserveVenueSlot(VenueSlotDetailsSearch venueSlotDetailsSearch, string resyToken = "");
        //ValueTask<ResyReservationCancellationResult> CancelReservationAsync(string reservationToken, string resyToken = "");
        ValueTask<RestaurantReservation> ReserveRestaurantAsync(RestaurantReservationCriteria restaurantReservationCriteria, string resyToken = "");
    }
}
