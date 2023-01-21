// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.Resy;

namespace BookMate.Core.Api.Brokers.APIs.Resy
{
    public partial class ResyApiBroker
    {
        const string BookSlotRelativeUrl = "3/book";

        public async ValueTask<ExternalResyVenueSlotBookingResult> BookVenueSlot(ExternalResyVenueSlotBookingDetails externalResyVenueSlotBookingDetails, string token = null)
        {
            var parameters = new Dictionary<string, string>
            {
                { "book_token", externalResyVenueSlotBookingDetails.BookingToken },
                { "struct_payment_method", externalResyVenueSlotBookingDetails.PaymentMethod },
            };

            return await this.PostAync<ExternalResyVenueSlotBookingResult>(
                relativeUrl: $"{BookSlotRelativeUrl}",
                parameters,
                token);
        }
    }
}
