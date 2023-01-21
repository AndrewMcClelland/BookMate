// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.Restaurants;
using BookMate.Core.Api.Services.Coordinations.Restaurants;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace BookMate.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : RESTFulController
    {
        private readonly IRestaurantCoordinationService restaurantCoordinationService;

        public RestaurantsController(IRestaurantCoordinationService restaurantCoordinationService) =>
            this.restaurantCoordinationService = restaurantCoordinationService;

        [HttpGet]
        public async ValueTask<ActionResult<List<Restaurant>>> SearchRestaurants([FromQuery] RestaurantSearchCriteria restaurantSearchCriteria)
        {
            List<Restaurant> restaurants = await this.restaurantCoordinationService.SearchRestaurantsAsync(restaurantSearchCriteria);

            return Ok(restaurants);
        }

        [HttpPost("reservation")]
        public async ValueTask<ActionResult<RestaurantReservation>> ReserveRestaurant([FromBody] RestaurantReservationCriteria restaurantReservationCriteria)
        {
            RestaurantReservation restaurantReservation = await this.restaurantCoordinationService.ReserveRestaurantAsync(restaurantReservationCriteria);

            return Ok(restaurantReservation);
        }
    }
}
