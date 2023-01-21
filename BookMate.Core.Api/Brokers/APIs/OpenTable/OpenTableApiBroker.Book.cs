// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.OpenTable;

namespace BookMate.Core.Api.Brokers.APIs.OpenTable
{
    public partial class OpenTableApiBroker : IOpenTableApiBroker
    {
        const string BookingPartialUrl = "dapi/booking/make-reservation";

        public async ValueTask<ExternalOpenTableBookingResult> BookRestaurant(ExternalOpenTableBooking externalOpenTableBooking)
        {
            return await this.PostAync<ExternalOpenTableBooking, ExternalOpenTableBookingResult>(
                relativeUrl: $"{BookingPartialUrl}",
                content: externalOpenTableBooking);
        }
    }
}
