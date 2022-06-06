// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

namespace BookMate.Core.Api.Tests.Acceptance.Brokers
{
    public partial class BookMateApiBroker
    {
        private const string HomeRelativeUrl = "api/home";

        public async ValueTask<string> GetHomeMessageAsync() => await this.apiFactoryClient.GetContentStringAsync(HomeRelativeUrl);
    }
}
