// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.Resy
{

    public class ExternalResyVenueSearchResult
    {
        [JsonProperty("search")]
        public ExternalResyVenueSearchResultSearch Search { get; set; }
    }

    public class ExternalResyVenueSearchResultSearch
    {
        [JsonProperty("nbHits")]
        public int NumberResults { get; set; }

        [JsonProperty("hits")]
        public ExternalResyVenueSearchResultResult[] Results { get; set; }
    }

    public class ExternalResyVenueSearchResultResult
    {
        [JsonProperty("contact")]
        public ExternalResyVenueSearchResultContactInformation ContactInformation { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("locality")]
        public string Locality { get; set; }

        [JsonProperty("id")]
        public ExternalResyId Id { get; set; }

        [JsonProperty("cuisine")]
        public string[] Cuisine { get; set; }

        [JsonProperty("rating")]
        public ExternalResyVenueSearchResultRating Rating { get; set; }

        [JsonProperty("_highlightResult")]
        public ExternalResyVenueSearchResultHighlightResult HighlightResult { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("location")]
        public ExternalResyVenueSearchResultLocation Location { get; set; }

        [JsonProperty("url_slug")]
        public string UrlSlug { get; set; }

        [JsonProperty("_geoloc")]
        public ExternalResyVenueCoordinates Coordinates { get; set; }

        [JsonProperty("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonProperty("price_range_id")]
        public int PriceRangeId { get; set; }

        [JsonProperty("cuisine_detail")]
        public string[] CuisineDetail { get; set; }

        [JsonProperty("images")]
        public string[] Images { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }
    }

    public class ExternalResyVenueSearchResultContactInformation
    {
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }

    public class ExternalResyId
    {
        [JsonProperty("resy")]
        public int Resy { get; set; }
    }

    public class ExternalResyVenueSearchResultRating
    {
        [JsonProperty("average")]
        public float? Average { get; set; }

        [JsonProperty("count")]
        public int? Count { get; set; }
    }

    public class ExternalResyVenueSearchResultHighlightResult
    {
        [JsonProperty("name")]
        public ExternalResyVenueSearchResultName Name { get; set; }
    }

    public class ExternalResyVenueSearchResultName
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("matchLevel")]
        public string MatchLevel { get; set; }

        [JsonProperty("fullyHighlighted")]
        public bool IsFullyHighlighted { get; set; }

        [JsonProperty("matchedWords")]
        public string[] MatchedWords { get; set; }
    }

    public class ExternalResyVenueSearchResultLocation
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
