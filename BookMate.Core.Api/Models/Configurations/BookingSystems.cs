// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;

namespace BookMate.Core.Api.Models.Configurations
{
    public class BookingSystems
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public IDictionary<string, string> PersistedQueryHashes { get; set; }
    }
}
