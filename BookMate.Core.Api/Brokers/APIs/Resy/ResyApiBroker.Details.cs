// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.Resy;

namespace BookMate.Core.Api.Brokers.APIs.Resy
{
    public partial class ResyApiBroker
    {
        const string SlotDetailsRelativeUrl = "3/details";

        public async ValueTask<ExternalResyVenueSlotDetailsSearchResult> GetVenueSlotDetails(ExternalResyVenueSlotDetailsSearch resyVenueSlotDetailsSearch, string token = null)
        {
            string relativeUrl = $"{SlotDetailsRelativeUrl}?" +
                $"config_id={resyVenueSlotDetailsSearch.ConfigId}&" +
                $"day={resyVenueSlotDetailsSearch.Date}&" +
                $"party_size={resyVenueSlotDetailsSearch.PartySize}";

            return await this.GetAsync<ExternalResyVenueSlotDetailsSearchResult>(relativeUrl, token);
        }
    }
}
