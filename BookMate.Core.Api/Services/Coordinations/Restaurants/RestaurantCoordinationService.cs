// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookMate.Core.Api.Brokers.Loggings;
using BookMate.Core.Api.Models.Restaurants;
using BookMate.Core.Api.Services.Orchestrations.OpenTable;
using BookMate.Core.Api.Services.Orchestrations.Resy;

namespace BookMate.Core.Api.Services.Coordinations.Restaurants
{
    public class RestaurantCoordinationService : IRestaurantCoordinationService
    {
        private readonly IOpenTableOrchestrationService openTableOrchestrationService;
        private readonly IResyOrchestrationService resyOrchestrationService;
        private readonly ILoggingBroker loggingBroker;

        public RestaurantCoordinationService(
            IOpenTableOrchestrationService openTableOrchestrationService,
            IResyOrchestrationService resyOrchestrationService,
            ILoggingBroker loggingBroker)
        {
            this.openTableOrchestrationService = openTableOrchestrationService;
            this.resyOrchestrationService = resyOrchestrationService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<List<Restaurant>> SearchRestaurantsAsync(RestaurantSearchCriteria restaurantSearchCriteria)
        {
            List<Restaurant> openTableRestaurants = await this.openTableOrchestrationService.SearchRestaurantsAsync(restaurantSearchCriteria);
            List<Restaurant> resyRestaurants = await this.resyOrchestrationService.SearchRestaurantsAsync(restaurantSearchCriteria);
            List<Restaurant> allRestaurants = openTableRestaurants.Concat(resyRestaurants).ToList();

            return allRestaurants;
        }

        public async ValueTask<RestaurantReservation> ReserveRestaurantAsync(RestaurantReservationCriteria restaurantReservationCriteria)
        {
            return restaurantReservationCriteria.BookingSystem switch
            {
                RestaurantBookingSystem.OpenTable => await this.openTableOrchestrationService.ReserveRestaurantAsync(restaurantReservationCriteria),
                RestaurantBookingSystem.Resy => await this.resyOrchestrationService.ReserveRestaurantAsync(restaurantReservationCriteria),
                _ => throw new Exception($"Invalid {nameof(RestaurantBookingSystem)} value: {restaurantReservationCriteria.BookingSystem}.")
            };
        }
    }
}
