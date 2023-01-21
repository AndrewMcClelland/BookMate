// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.OpenTable
{
    public class ExternalOpenTableRestaurantsAvailabilityCriteria
    {
        [JsonProperty("operationName")]
        public string OperationName { get; set; }

        [JsonProperty("variables")]
        public ExternalOpenTableRestaurantsAvailabilityCriteriaVariables Variables { get; set; }

        [JsonProperty("extensions")]
        public ExternalOpenTableExtensions Extensions { get; set; }
    }

    public class ExternalOpenTableRestaurantsAvailabilityCriteriaVariables
    {
        [JsonProperty("restaurantIds")]
        public int[] RestaurantIds { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("partySize")]
        public int PartySize { get; set; }

        [JsonProperty("databaseRegion")]
        public string DatabaseRegion { get; set; }

        [JsonProperty("restaurantAvailabilityTokens")]
        public string[] RestaurantAvailabilityTokens { get; set; }
    }
}