// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookMate.Core.Api.Brokers.Loggings;
using BookMate.Core.Api.Models.Externals.Resy;
using BookMate.Core.Api.Models.Restaurants;
using BookMate.Core.Api.Models.Resy;
using BookMate.Core.Api.Services.Foundations.Resy;

namespace BookMate.Core.Api.Services.Orchestrations.Resy
{
    public class ResyOrchestrationService : IResyOrchestrationService
    {
        private readonly IResyService resyService;
        private readonly ILoggingBroker loggingBroker;

        public ResyOrchestrationService(IResyService resyService, ILoggingBroker loggingBroker)
        {
            this.resyService = resyService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<ExternalResyProfile> AuthenticateWithCredentialsAsync(string email, string password) =>
            await this.resyService.AuthenticateWithCredentialsAsync(email, password);

        public async ValueTask<List<Restaurant>> SearchRestaurantsAsync(RestaurantSearchCriteria restaurantSearchCriteria) =>
            await this.resyService.SearchRestaurantsAsync(restaurantSearchCriteria);

        //public async ValueTask<ResyVenueSlotsSearchResult> FindVenueSlotsAsync(VenueSlotsSearch venueSlotsSearch, string resyToken = "") =>
        //    await this.resyService.FindVenueSlotsAsync(venueSlotsSearch, resyToken);

        //public async ValueTask<ResyVenueSlotBookingResult> ReserveVenueSlot(VenueSlotDetailsSearch venueSlotDetailsSearch, string resyToken = "")
        //{
        //    ResyVenueSlotDetailsSearchResult externalResyVenueSlotDetailsSearchResult = await this.resyService.GetVenueSlotDetailsAsync(venueSlotDetailsSearch, resyToken);

        //    var venueSlotBookingDetails = new VenueSlotBookingDetails
        //    {
        //        BookingToken = externalResyVenueSlotDetailsSearchResult.book_token.value,
        //        PaymentMethodId = externalResyVenueSlotDetailsSearchResult.user.PaymentMethods.First().Id.ToString()
        //    };

        //    return await this.resyService.BookVenueSlotAsync(venueSlotBookingDetails, resyToken);
        //}

        //public async ValueTask<ResyReservationCancellationResult> CancelReservationAsync(string reservationToken, string resyToken = "") =>
        //    await this.resyService.CancelReservationAsync(reservationToken, resyToken);

        public async ValueTask<RestaurantReservation> ReserveRestaurantAsync(RestaurantReservationCriteria restaurantReservationCriteria, string resyToken = "")
        {
            List<ResyRestaurantSlot> restaurantSlots = await this.resyService.SearchRestaurantAvailabilityAsync(restaurantReservationCriteria);

            // TODO: Figure out which slot is optimal
            ResyRestaurantSlot desiredSlot = restaurantSlots.First();

            var resyRestaurantSlotDetailsSearch = new ResyRestaurantSlotDetailsSearch
            {
                ConfigId = desiredSlot.Token,
                PartySize = desiredSlot.PartySize,
                Date = desiredSlot.StartDateTime
            };

            ResyRestaurantSlotDetails resyRestaurantSlotDetails = await this.resyService.GetRestaurantSlotDetailsAsync(resyRestaurantSlotDetailsSearch, resyToken);

            var resyRestaurantSlotBookingCriteria = new ResyRestaurantSlotBookingCriteria
            {
                BookingToken = resyRestaurantSlotDetails.BookingToken,
                PaymentMethodId = resyRestaurantSlotDetails.PaymentMethodId
            };

            ResyRestaurantSlotBooking resyRestaurantSlotBooking = await this.resyService.BookRestaurantSlotAsync(resyRestaurantSlotBookingCriteria, resyToken);

            var restaurantReservation = new RestaurantReservation
            {
                BookingSystem = RestaurantBookingSystem.Resy,
                RestaurantId = restaurantReservationCriteria.RestaurantId,
                ReservationId = resyRestaurantSlotBooking.ReservationId,
                ReservationToken = resyRestaurantSlotBooking.Token,
                DateTime = desiredSlot.StartDateTime,
                PartySize = desiredSlot.PartySize,
                Environment = desiredSlot.Type
            };

            return restaurantReservation;
        }
    }
}
