// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.OpenTable
{
    public class ExternalOpenTableSearchResult
    {
        [JsonProperty("data")]
        public ExternalOpenTableSearchResultData Data { get; set; }
    }

    public class ExternalOpenTableSearchResultData
    {
        [JsonProperty("autocomplete")]
        public ExternalOpenTableSearchResultAutoComplete AutoComplete { get; set; }
    }

    public class ExternalOpenTableSearchResultAutoComplete
    {
        [JsonProperty("autocompleteResults")]
        public ExternalOpenTableSearchResultAutoCompleteResult[] AutoCompleteResults { get; set; }

        [JsonProperty("correlationId")]
        public string CorrelationId { get; set; }
    }

    public class ExternalOpenTableSearchResultAutoCompleteResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("metroId")]
        public int MetroId { get; set; }

        [JsonProperty("metroName")]
        public string MetroName { get; set; }

        [JsonProperty("macroId")]
        public int MacroId { get; set; }

        [JsonProperty("macroName")]
        public string MacroName { get; set; }

        [JsonProperty("neighborhoodName")]
        public string NeighborhoodName { get; set; }

        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }
    }
}
