// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookMate.Core.Api.Brokers.APIs.OpenTable;
using BookMate.Core.Api.Brokers.Loggings;
using BookMate.Core.Api.Models.Externals.OpenTable;
using BookMate.Core.Api.Models.Restaurants;

namespace BookMate.Core.Api.Services.Foundations.OpenTable
{
    public class OpenTableService : IOpenTableService
    {
        private readonly IOpenTableApiBroker openTableApiBroker;
        private readonly ILoggingBroker loggingBroker;

        public OpenTableService(
            IOpenTableApiBroker openTableApiBroker,
            ILoggingBroker loggingBroker)
        {
            this.openTableApiBroker = openTableApiBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<List<Restaurant>> SearchRestaurantsAsync(RestaurantSearchCriteria restaurantSearchCriteria)
        {
            var externalOpenTableSearchCriteria = new ExternalOpenTableSearchCriteria
            {
                OperationName = "Autocomplete",
                Variables = new ExternalOpenTableSearchCriteriaVariables
                {
                    Query = restaurantSearchCriteria.Name,
                    Latitude = restaurantSearchCriteria.Coordinates.Latitude,
                    Longitude = restaurantSearchCriteria.Coordinates.Longitude,
                    UseNewVersion = true
                }
            };

            ExternalOpenTableSearchResult externalOpenTableSearchResult = await this.openTableApiBroker.SearchRestaurants(externalOpenTableSearchCriteria);

            List<Restaurant> restaurants = externalOpenTableSearchResult?.Data?.AutoComplete?.AutoCompleteResults?.Select(result =>
                new Restaurant
                {
                    BookingSystem = RestaurantBookingSystem.OpenTable,
                    Id = result.Id,
                    Name = result.Name,

                    Location = new RestaurantLocation
                    {
                        Neighborhood = result.NeighborhoodName,
                        Borough = result.MacroName,
                        County = result.MetroName,
                        Country = result.Country,

                        Coordinates = new RestaurantCoordinates
                        {
                            Latitude = result.Latitude,
                            Longitude = result.Longitude
                        }
                    }
                }
            ).ToList();

            return restaurants;
        }

        public async ValueTask<ExternalOpenTableRestaurantsAvailabilityResult> SearchRestaurantAvailabilityAsync(RestaurantReservationCriteria restaurantReservationCriteria)
        {
            var externalOpenTableRestaurantsAvailabilityCriteria = new ExternalOpenTableRestaurantsAvailabilityCriteria
            {
                Variables = new ExternalOpenTableRestaurantsAvailabilityCriteriaVariables
                {
                    RestaurantIds = new int[]
                    {
                        int.Parse(restaurantReservationCriteria.RestaurantId)
                    },

                    PartySize = restaurantReservationCriteria.PartySize,
                    Date = restaurantReservationCriteria.DateTime.ToString("yyyy-MM-dd"),
                    Time = restaurantReservationCriteria.DateTime.ToString("HH:mm"),
                    DatabaseRegion = "NA",

                    RestaurantAvailabilityTokens = new string[]
                    {
                        "eyJ2IjoyLCJtIjoxLCJwIjowLCJzIjowLCJuIjowfQ"
                    }
                }
            };

            return await this.openTableApiBroker.SearchRestaurantsAvailability(externalOpenTableRestaurantsAvailabilityCriteria);
        }

        public async ValueTask<ExternalOpenTableSlotLockBookingResult> BookRestaurantLockSlotAsync(ExternalOpenTableSlotLockBooking externalOpenTableSlotLockBooking)
        {
            externalOpenTableSlotLockBooking.Variables.SlotLockInput.DatabaseRegion = "NA";

            return await this.openTableApiBroker.BookRestaurantSlotLock(externalOpenTableSlotLockBooking);
        }

        public async ValueTask<ExternalOpenTableBookingResult> BookRestaurantAsyncAsync(ExternalOpenTableBooking externalOpenTableBooking)
        {
            return await this.openTableApiBroker.BookRestaurant(externalOpenTableBooking);
        }
    }
}
