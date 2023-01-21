// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using BookMate.Core.Api.Services.Orchestrations.Resy;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace BookMate.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResyController : RESTFulController
    {
        private readonly IResyOrchestrationService resyOrchestrationService;

        public ResyController(IResyOrchestrationService resyOrchestrationService) =>
            this.resyOrchestrationService = resyOrchestrationService;

        //[HttpPost("auth/credentials")]
        //public async ValueTask<ActionResult<ResyProfile>> AuthenticateResyProfile(string email, string password)
        //{
        //    ResyProfile externalResyProfile = await this.resyOrchestrationService.AuthenticateWithCredentialsAsync(email, password);

        //    return Ok(externalResyProfile);
        //}

        //[HttpGet("venues")]
        //public async ValueTask<ActionResult<List<Venue>>> SearchVenues([FromQuery] VenueSearchCriteria venueSearchCriteria, string authToken = null)
        //{
        //    List<Venue> venues = await this.resyOrchestrationService.SearchAllVenuesAsync(venueSearchCriteria, authToken);

        //    return Ok(venues);
        //}

        //[HttpGet("venues/slots")]
        //public async ValueTask<ActionResult<ResyVenueSlotsSearchResult>> SearchVenueSlots([FromQuery] VenueSlotsSearch venueSlotsSearch, string authToken = null)
        //{
        //    ResyVenueSlotsSearchResult externalResyVenueSlotsSearchResult = await this.resyOrchestrationService.FindVenueSlotsAsync(venueSlotsSearch, authToken);

        //    return Ok(externalResyVenueSlotsSearchResult);
        //}

        //[HttpGet("venues/slots/details")]
        //public async ValueTask<ActionResult<ResyVenueSlotsSearchResult>> GetVenueSlotDetails([FromQuery] VenueSlotDetailsSearch venueSlotDetailsSearch, string authToken = null)
        //{
        //    ResyVenueSlotDetailsSearchResult externalResyVenueSlotDetailsSearchResult = await this.resyService.GetVenueSlotDetailsAsync(venueSlotDetailsSearch, authToken);

        //    return Ok(externalResyVenueSlotDetailsSearchResult);
        //}

        //[HttpPost("reserve")]
        //public async ValueTask<ActionResult<ResyVenueSlotBookingResult>> ReserveVenueSlot(VenueSlotDetailsSearch venueSlotDetailsSearch, string authToken = null)
        //{
        //    ResyVenueSlotBookingResult externalResyVenueSlotBookingResult = await this.resyOrchestrationService.ReserveVenueSlot(venueSlotDetailsSearch, authToken);

        //    return Ok(externalResyVenueSlotBookingResult);
        //}


        // This currently doesn't work :( The resy_token we get from the reservation response isn't exactly the same as what is passed to cancellation endpoint
        // The very last part of the reservation response token is at the end of the cancellation token, but before that I don't know where it's from
        //[HttpPost("cancel")]
        //public async ValueTask<ActionResult<ResyReservationCancellationResult>> CancelReservation(string reservationToken, string authToken = null)
        //{
        //    ResyReservationCancellationResult externalResyReservationCancellationResult = await this.resyOrchestrationService.CancelReservationAsync(reservationToken, authToken);

        //    return Ok(externalResyReservationCancellationResult);
        //}
    }
}
