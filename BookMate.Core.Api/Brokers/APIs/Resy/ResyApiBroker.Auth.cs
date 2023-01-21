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
        const string EmailAuthRelativeUrl = "3/auth/password";
        const string MobileAuthRelativeUrl = "3/auth/mobile";

        public async ValueTask<ExternalResyProfile> AuthenticateCredentials(ExternalResyCredentials externalResyCredentials)
        {
            var parameters = new Dictionary<string, string>
            {
                { "email", externalResyCredentials.Email },
                { "password", externalResyCredentials.Password },
            };

            return await this.PostAync<ExternalResyProfile>(
                relativeUrl: $"{EmailAuthRelativeUrl}",
                parameters);
        }
    }
}
