// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.OpenTable
{
    public class ExternalOpenTableBooking
    {
        [JsonProperty("restaurantId")]
        public int RestaurantId { get; set; }

        [JsonProperty("slotAvailabilityToken")]
        public string SlotAvailabilityToken { get; set; }

        [JsonProperty("slotHash")]
        public string SlotHash { get; set; }

        [JsonProperty("slotLockId")]
        public int SlotLockId { get; set; }

        [JsonProperty("reservationDateTime")]
        public string ReservationDateTime { get; set; }

        [JsonProperty("partySize")]
        public int PartySize { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("reservationType")]
        public string ReservationType { get; set; }

        [JsonProperty("reservationAttribute")]
        public string ReservationAttribute { get; set; }

        [JsonProperty("pointsType")]
        public string PointsType { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("phoneNumberCountryId")]
        public string PhoneNumberCountryId { get; set; }

        [JsonProperty("optInEmailRestaurant")]
        public bool OptInEmailRestaurant { get; set; }
    }
}
