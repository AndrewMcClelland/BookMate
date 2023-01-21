// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;

namespace BookMate.Core.Api.Models.Configurations
{
    public class LocalConfigurations
    {
        public IDictionary<string, BookingSystems> BookingSystems { get; set; }
        public TwilioConfiguration TwilioConfiguration { get; set; }
        public string UserAgent { get; set; }
    }
}
