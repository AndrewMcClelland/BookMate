// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.OpenTable;

namespace BookMate.Core.Api.Brokers.APIs.OpenTable
{
    public partial class OpenTableApiBroker : IOpenTableApiBroker
    {
        const string RestaurantsAvailabilityOperationName = "RestaurantsAvailability";

        public async ValueTask<ExternalOpenTableRestaurantsAvailabilityResult> SearchRestaurantsAvailability(ExternalOpenTableRestaurantsAvailabilityCriteria externalOpenTableRestaurantsAvailabilityCriteria)
        {
            externalOpenTableRestaurantsAvailabilityCriteria.Extensions = new ExternalOpenTableExtensions
            {
                PersistedQuery = new ExternalOpenTablePersistedQuery
                {
                    Version = 1,
                    Sha256Hash = this.persistedQueryHashes[RestaurantsAvailabilityOperationName]
                }
            };

            string relativeUrl = $"{GraphQlRelativeUrl}?" +
                $"optype={QueryOperationType}&" +
                $"opname={RestaurantsAvailabilityOperationName}";

            return await this.PostAync<ExternalOpenTableRestaurantsAvailabilityCriteria, ExternalOpenTableRestaurantsAvailabilityResult>(relativeUrl, externalOpenTableRestaurantsAvailabilityCriteria);
        }
    }
}
