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
        const string CancelReservationRelativeUrl = "3/cancel";

        public async ValueTask<ExternalResyReservationCancellationResult> CancelReservation(string reservationToken, string authToken = null)
        {
            var parameters = new Dictionary<string, string>
            {
                { "action_token", reservationToken}
            };

            return await this.PostAync<ExternalResyReservationCancellationResult>(
                relativeUrl: $"{CancelReservationRelativeUrl}",
                parameters,
                authToken);
        }
    }
}
