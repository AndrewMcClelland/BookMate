// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.OpenTable
{
    public class ExternalOpenTableBookingResult
    {
        [JsonProperty("reservationId")]
        public int ReservationId { get; set; }

        [JsonProperty("restaurantId")]
        public int RestaurantId { get; set; }

        [JsonProperty("reservationDateTime")]
        public string ReservationDateTime { get; set; }

        [JsonProperty("partySize")]
        public int PartySize { get; set; }

        [JsonProperty("confirmationNumber")]
        public int ConfirmationNumber { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("reservationStateId")]
        public int ReservationStateId { get; set; }

        [JsonProperty("securityToken")]
        public string SecurityToken { get; set; }

        [JsonProperty("gpid")]
        public long Gpid { get; set; }

        [JsonProperty("reservationHash")]
        public string ReservationHash { get; set; }

        [JsonProperty("reservationType")]
        public string ReservationType { get; set; }

        [JsonProperty("reservationSource")]
        public string ReservationSource { get; set; }

        [JsonProperty("creditCardLastFour")]
        public string CreditCardLastFour { get; set; }

        [JsonProperty("userType")]
        public int UserType { get; set; }

        [JsonProperty("diningAreaId")]
        public int DiningAreaId { get; set; }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
