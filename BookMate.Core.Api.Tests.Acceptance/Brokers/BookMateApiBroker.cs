// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Microsoft.AspNetCore.Mvc.Testing;
using RESTFulSense.Clients;

namespace BookMate.Core.Api.Tests.Acceptance.Brokers
{
    public partial class BookMateApiBroker
    {
        private readonly WebApplicationFactory<Startup> webApplicationFactory;
        private readonly HttpClient httpClient;
        private readonly IRESTFulApiFactoryClient apiFactoryClient;

        public BookMateApiBroker()
        {
            this.webApplicationFactory = new WebApplicationFactory<Startup>();
            this.httpClient = this.webApplicationFactory.CreateClient();
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.httpClient);
        }
    }
}
