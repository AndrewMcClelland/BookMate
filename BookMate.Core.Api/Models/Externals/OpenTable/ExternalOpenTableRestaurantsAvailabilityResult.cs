// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.OpenTable
{

    public class ExternalOpenTableRestaurantsAvailabilityResult
    {
        [JsonProperty("data")]
        public ExternalOpenTableRestaurantsAvailabilityResultData Data { get; set; }
    }

    public class ExternalOpenTableRestaurantsAvailabilityResultData
    {
        [JsonProperty("availability")]
        public ExternalOpenTableRestaurantAvailability[] Availability { get; set; }
    }

    public class ExternalOpenTableRestaurantAvailability
    {
        [JsonProperty("restaurantId")]
        public int RestaurantId { get; set; }

        [JsonProperty("restaurantAvailabilityToken")]
        public string RestaurantAvailabilityToken { get; set; }

        [JsonProperty("availabilityDays")]
        public ExternalOpenTableRestaurantAvailabilityDay[] AvailabilityDays { get; set; }
    }

    public class ExternalOpenTableRestaurantAvailabilityDay
    {
        [JsonProperty("noTimesReasons")]
        public string[] NoTimesReasons { get; set; }

        [JsonProperty("dayOffset")]
        public int DayOffset { get; set; }

        [JsonProperty("allowNextAvailable")]
        public bool AllowNextAvailable { get; set; }

        [JsonProperty("slots")]
        public ExternalOpenTableRestaurantAvailabilitySlot[] slots { get; set; }
    }

    public class ExternalOpenTableRestaurantAvailabilitySlot
    {
        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonProperty("timeOffsetMinutes")]
        public int TimeOffsetMinutes { get; set; }

        [JsonProperty("slotHash")]
        public string SlotHash { get; set; }

        [JsonProperty("slotAvailabilityToken")]
        public string SlotAvailabilityToken { get; set; }

        [JsonProperty("attributes")]
        public string[] Attributes { get; set; }

        [JsonProperty("pointsType")]
        public string PointsType { get; set; }
    }
}
