// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.Resy;

namespace BookMate.Core.Api.Brokers.APIs.Resy
{
    public partial class ResyApiBroker : IResyApiBroker
    {
        const string VenueSearchRelativeUrl = "3/venuesearch/search";

        public async ValueTask<ExternalResyVenueSearchResult> SearchAllVenues(ExternalResyVenueSearch externalResyVenueSearch, string token = "")
        {
            return await this.PostAync<ExternalResyVenueSearch, ExternalResyVenueSearchResult>(
                relativeUrl: $"{VenueSearchRelativeUrl}",
                content: externalResyVenueSearch,
                token);
        }
    }
}
