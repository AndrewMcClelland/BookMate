// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RESTFulSense.Clients;
using RestSharp;

namespace BookMate.Core.Api.Brokers.APIs.Resy
{
    public partial class ResyApiBroker : IResyApiBroker
    {
        private readonly HttpClient httpClient;
        private readonly IRESTFulApiFactoryClient apiClient;
        private readonly RestClient restClient;

        public ResyApiBroker(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.apiClient = GetApiClient(configuration);
            this.restClient = GetRestClient(configuration);
        }

        private async ValueTask<U> PostAync<T, U>(string relativeUrl, T content, string token = "") where T : class
        {
            // RestfulSense: How to add custom headers
            //await this.apiClient.PostContentAsync<T, U>(relativeUrl, content);

            var request = new RestRequest(relativeUrl, Method.Post);
            request.AddHeader("x-resy-auth-token", token);
            request.AddJsonBody(content);
            RestResponse response = this.restClient.Execute(request);

            return JsonConvert.DeserializeObject<U>(response.Content);
        }

        private async ValueTask<T> PostAync<T>(string relativeUrl, IDictionary<string, string> parameters, string token = "")
        {
            var request = new RestRequest(relativeUrl, Method.Post);
            request.AddHeader("x-resy-auth-token", token);

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }

            RestResponse response = this.restClient.Execute(request);

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        private async ValueTask<T> GetAsync<T>(string relativeUrl, string token = "")
        {
            // RestfulSense: How to add custom headers?
            //await apiClient.GetContentAsync<T>(relativeUrl);

            var request = new RestRequest(relativeUrl, Method.Get);
            request.AddHeader("x-resy-auth-token", token);
            RestResponse response = this.restClient.Execute(request);

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        private IRESTFulApiFactoryClient GetApiClient(IConfiguration configuration)
        {
            LocalConfigurations localConfigurations = configuration.Get<LocalConfigurations>();
            string baseUrl = localConfigurations.BookingSystems["Resy"].BaseUrl;
            string apiKey = localConfigurations.BookingSystems["Resy"].ApiKey;
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"ResyAPI api_key=\"{apiKey}\"");
            var productInfoHeaderValue = new ProductInfoHeaderValue("Test", "0.1");
            httpClient.DefaultRequestHeaders.UserAgent.Add(productInfoHeaderValue);

            return new RESTFulApiFactoryClient(httpClient);
        }

        private static RestClient GetRestClient(IConfiguration configuration)
        {
            LocalConfigurations localConfigurations = configuration.Get<LocalConfigurations>();
            string baseUrl = localConfigurations.BookingSystems["Resy"].BaseUrl;
            string apiKey = localConfigurations.BookingSystems["Resy"].ApiKey;
            var restClient = new RestClient(baseUrl);
            restClient.AddDefaultHeader("authorization", $"ResyAPI api_key=\"{apiKey}\"");

            return restClient;
        }
    }
}
