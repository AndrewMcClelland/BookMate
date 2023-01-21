// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.OpenTable
{
    public class ExternalOpenTableSlotLockBooking
    {
        [JsonProperty("operationName")]
        public string OperationName { get; set; }

        [JsonProperty("variables")]
        public ExternalOpenTableSlotLockBookingVariables Variables { get; set; }

        [JsonProperty("extensions")]
        public ExternalOpenTableExtensions Extensions { get; set; }
    }

    public class ExternalOpenTableSlotLockBookingVariables
    {
        [JsonProperty("slotLockInput")]
        public ExternalOpenTableSlotLockBookingSlotLockInput SlotLockInput { get; set; }
    }

    public class ExternalOpenTableSlotLockBookingSlotLockInput
    {
        [JsonProperty("restaurantId")]
        public int RestaurantId { get; set; }

        [JsonProperty("seatingOption")]
        public string SeatingOption { get; set; }

        [JsonProperty("reservationDateTime")]
        public string ReservationDateTime { get; set; }

        [JsonProperty("partySize")]
        public int PartySize { get; set; }

        [JsonProperty("databaseRegion")]
        public string DatabaseRegion { get; set; }

        [JsonProperty("slotHash")]
        public string SlotHash { get; set; }

        [JsonProperty("reservationType")]
        public string ReservationType { get; set; }
    }
}
