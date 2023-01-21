// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.OpenTable
{
    public class ExternalOpenTableSearchCriteria
    {
        [JsonProperty("operationName")]
        public string OperationName { get; set; }

        [JsonProperty("variables")]
        public ExternalOpenTableSearchCriteriaVariables Variables { get; set; }

        [JsonProperty("extensions")]
        public ExternalOpenTableExtensions Extensions { get; set; }
    }

    public class ExternalOpenTableSearchCriteriaVariables
    {
        [JsonProperty("term")]
        public string Query { get; set; }

        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }

        [JsonProperty("useNewVersion")]
        public bool UseNewVersion { get; set; }
    }

    public class ExternalOpenTableExtensions
    {
        [JsonProperty("persistedQuery")]
        public ExternalOpenTablePersistedQuery PersistedQuery { get; set; }
    }

    public class ExternalOpenTablePersistedQuery
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("sha256Hash")]
        public string Sha256Hash { get; set; }
    }
}
