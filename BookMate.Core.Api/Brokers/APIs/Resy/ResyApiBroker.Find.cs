// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.Resy;

namespace BookMate.Core.Api.Brokers.APIs.Resy
{
    public partial class ResyApiBroker
    {
        const string FindRelativeUrl = "4/find";

        public async ValueTask<ExternalResyVenueSlotsSearchResult> FindVenueSlots(ExternalResyVenueSlotsSearch externalResyVenueSlotsSearch, string token)
        {
            string relativeUrl = $"{FindRelativeUrl}?" +
                $"venue_id={externalResyVenueSlotsSearch.Id}&" +
                $"day={externalResyVenueSlotsSearch.Date}&" +
                $"party_size={externalResyVenueSlotsSearch.PartySize}&" +
                $"lat={externalResyVenueSlotsSearch.Latitude}&" +
                $"long={externalResyVenueSlotsSearch.Longitude}";

            return await this.GetAsync<ExternalResyVenueSlotsSearchResult>(relativeUrl, token);
        }
    }
}
