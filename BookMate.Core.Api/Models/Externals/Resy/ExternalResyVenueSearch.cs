// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Text.Json.Serialization;

namespace BookMate.Core.Api.Models.Externals.Resy
{
    // Need to use 'JsonPropertyName' instead of 'JsonProperty' due to the way RestSharp serializes JSON content
    public class ExternalResyVenueSearch
    {
        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("geo")]
        public ExternalResyVenueCoordinates ResyVenueCoordinates { get; set; }

        [JsonPropertyName("per_page")]
        public int NumberResults { get; set; }

        [JsonPropertyName("types")]
        public string[] VenueTypes { get; set; }

        //[JsonPropertyName("slot_filter")]
        //public ExternalResySlotFilter SlotFilter { get; set; }
    }

    public class ExternalResyVenueCoordinates
    {
        [JsonPropertyName("latitude")]
        public float Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public float Longitude { get; set; }
    }

    public class ExternalResySlotFilter
    {
        [JsonPropertyName("day")]
        public string Date { get; set; }

        [JsonPropertyName("party_size")]
        public int PartySize { get; set; }
    }
}
