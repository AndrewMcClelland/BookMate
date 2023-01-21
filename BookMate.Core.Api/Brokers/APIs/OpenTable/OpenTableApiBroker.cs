// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RESTFulSense.Clients;
using RestSharp;

namespace BookMate.Core.Api.Brokers.APIs.OpenTable
{
    public partial class OpenTableApiBroker : IOpenTableApiBroker
    {
        private readonly HttpClient httpClient;
        private readonly IRESTFulApiFactoryClient apiClient;
        private readonly RestClient restClient;
        private readonly IDictionary<string, string> persistedQueryHashes;

        const string GraphQlRelativeUrl = "dapi/fe/gql";
        const string QueryOperationType = "query";
        const string MutationOperationType = "mutation";

        public OpenTableApiBroker(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.apiClient = GetApiClient(configuration);
            this.restClient = GetRestClient(configuration);
            this.persistedQueryHashes = GetPersistedQueryHashes(configuration);
        }

        private async ValueTask<U> PostAync<T, U>(string relativeUrl, T content, string token = "") where T : class
        {
            var request = new RestRequest(relativeUrl, Method.Post);
            var body = JsonConvert.SerializeObject(content);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            //request.AddJsonBody(content);
            RestResponse response = this.restClient.Execute(request);

            return JsonConvert.DeserializeObject<U>(response.Content);
        }

        private async ValueTask<T> PostAync<T>(string relativeUrl, IDictionary<string, string> parameters, string token = "")
        {
            var request = new RestRequest(relativeUrl, Method.Post);

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
            RestResponse response = this.restClient.Execute(request);

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        private IRESTFulApiFactoryClient GetApiClient(IConfiguration configuration)
        {
            LocalConfigurations localConfigurations = configuration.Get<LocalConfigurations>();
            string baseUrl = localConfigurations.BookingSystems["OpenTable"].BaseUrl;
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Add("x-query-timeout", "1500");
            httpClient.DefaultRequestHeaders.Add("x-csrf-token", "");

            return new RESTFulApiFactoryClient(httpClient);
        }

        private static RestClient GetRestClient(IConfiguration configuration)
        {
            LocalConfigurations localConfigurations = configuration.Get<LocalConfigurations>();
            string baseUrl = localConfigurations.BookingSystems["OpenTable"].BaseUrl;
            var restClient = new RestClient(baseUrl);
            restClient.AddDefaultHeader("x-query-timeout", "1500");
            restClient.AddDefaultHeader("x-csrf-token", "");

            return restClient;
        }
        private static IDictionary<string, string> GetPersistedQueryHashes(IConfiguration configuration)
        {
            LocalConfigurations localConfigurations = configuration.Get<LocalConfigurations>();

            return localConfigurations.BookingSystems["OpenTable"].PersistedQueryHashes;
        }
    }
}

// Below is the suspected query format that's hashed
//query Autocomplete(term: String,  latitude: Float, longitude: Float, useNewVersion: Boolean) {
//    autocompleteResults(term: $term, latitude: $latitude, longitude: $longitude, useNewVersion: $useNewVersion) {
//        id
//        type
//        locationSubtype
//        name
//        country
//        countryId
//        metroId
//        metroName
//        macroId
//        macroName
//        neighborhoodName
//        latitude
//        longitude
//        query
//        __typename
//    }
//    correlationId
//    __typename
//}