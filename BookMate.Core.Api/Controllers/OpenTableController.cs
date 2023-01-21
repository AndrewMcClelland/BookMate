// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using BookMate.Core.Api.Services.Orchestrations.OpenTable;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace BookMate.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenTableController : RESTFulController
    {
        private readonly IOpenTableOrchestrationService openTableOrchestrationService;

        public OpenTableController(IOpenTableOrchestrationService openTableOrchestrationService) =>
            this.openTableOrchestrationService = openTableOrchestrationService;

        //[HttpGet("restaurants")]
        //public async ValueTask<ActionResult<List<Venue>>> SearchRestaurants([FromQuery] VenueSearchCriteria venueSearchCriteria)
        //{
        //    List<Venue> venues = await this.openTableService.SearchRestaurantsAsync(venueSearchCriteria);

        //    return Ok(venues);
        //}

        //[HttpGet("restaurants/availability")]
        //public async ValueTask<ActionResult<OpenTableRestaurantsAvailabilityResult>> SearchRestaurantAvailability([FromQuery] VenueAvailabilityCriteria venueAvailabilityCriteria)
        //{
        //    OpenTableRestaurantsAvailabilityResult externalOpenTableRestaurantsAvailabilityResult = await this.openTableOrchestrationService.SearchRestaurantsAsync(venueAvailabilityCriteria);

        //    return Ok(externalOpenTableRestaurantsAvailabilityResult);
        //}

        //[HttpPost("restaurants/book")]
        //public async ValueTask<ActionResult<OpenTableSlotLockBookingResult>> BookRestaurantSlot([FromQuery] OpenTableSlotLockBooking externalOpenTableSlotLockBooking)
        //{
        //    OpenTableSlotLockBookingResult externalOpenTableSlotLockBookingResult = await this.openTableOrchestrationService.ReserveRestaurantAsync(externalOpenTableSlotLockBooking);

        //    return Ok(externalOpenTableSlotLockBookingResult);
        //}
    }
}
