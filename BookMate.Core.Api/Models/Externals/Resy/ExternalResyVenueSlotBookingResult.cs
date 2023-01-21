// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.Resy
{
    public class ExternalResyVenueSlotBookingResult

    {
        [JsonProperty("resy_token")]
        public string Token { get; set; }

        [JsonProperty("reservation_id")]
        public int ReservationId { get; set; }
    }

}
