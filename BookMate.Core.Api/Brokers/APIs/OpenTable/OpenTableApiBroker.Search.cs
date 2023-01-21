// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.OpenTable;

namespace BookMate.Core.Api.Brokers.APIs.OpenTable
{
    public partial class OpenTableApiBroker : IOpenTableApiBroker
    {
        const string SearchOperationName = "Autocomplete";

        public async ValueTask<ExternalOpenTableSearchResult> SearchRestaurants(ExternalOpenTableSearchCriteria externalOpenTableSearchCriteria)
        {
            externalOpenTableSearchCriteria.Extensions = new ExternalOpenTableExtensions
            {
                PersistedQuery = new ExternalOpenTablePersistedQuery
                {
                    Version = 1,
                    Sha256Hash = this.persistedQueryHashes[SearchOperationName]
                }
            };

            string relativeUrl = $"{GraphQlRelativeUrl}?" +
                $"optype={QueryOperationType}&" +
                $"opname={SearchOperationName}";

            return await this.PostAync<ExternalOpenTableSearchCriteria, ExternalOpenTableSearchResult>(relativeUrl, externalOpenTableSearchCriteria);
        }
    }
}
