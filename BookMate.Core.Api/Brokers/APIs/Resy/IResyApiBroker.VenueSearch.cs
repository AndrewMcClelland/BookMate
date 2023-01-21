// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.Resy;

namespace BookMate.Core.Api.Brokers.APIs.Resy
{
    public partial interface IResyApiBroker
    {
        ValueTask<ExternalResyVenueSearchResult> SearchAllVenues(ExternalResyVenueSearch externalResyVenueSearch, string token);
    }
}
