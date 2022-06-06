// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.Configurations;
using Microsoft.Extensions.Configuration;
using RESTFulSense.Clients;

namespace BookMate.Core.Api.Brokers.BookingSystems.ForeUpSoftwareBookingSystems
{
    public partial class ForeUpSoftwareBookingSystemBroker : IForeUpSoftwareBookingSystemBroker
    {
        private readonly IRESTFulApiFactoryClient apiClient;
        private readonly HttpClient httpClient;

        public ForeUpSoftwareBookingSystemBroker(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            apiClient = GetApiClient(configuration);
        }

        private async ValueTask<T> GetAsync<T>(string relativeUrl) =>
            await apiClient.GetContentAsync<T>(relativeUrl);

        private IRESTFulApiFactoryClient GetApiClient(IConfiguration configuration)
        {
            LocalConfigurations localConfigurations = configuration.Get<LocalConfigurations>();
            string baseUrl = localConfigurations.BookingSystems["ForeUpSoftware"].BaseUrl;
            httpClient.BaseAddress = new Uri(baseUrl);

            return new RESTFulApiFactoryClient(httpClient);
        }
    }
}
