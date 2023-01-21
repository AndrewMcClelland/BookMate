// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.OpenTable;

namespace BookMate.Core.Api.Brokers.APIs.OpenTable
{
    public partial class OpenTableApiBroker : IOpenTableApiBroker
    {
        const string BookSlotLockOperationName = "BookDetailsStandardSlotLock";

        public async ValueTask<ExternalOpenTableSlotLockBookingResult> BookRestaurantSlotLock(ExternalOpenTableSlotLockBooking externalOpenTableSlotLockBooking)
        {
            externalOpenTableSlotLockBooking.Extensions = new ExternalOpenTableExtensions
            {
                PersistedQuery = new ExternalOpenTablePersistedQuery
                {
                    Version = 1,
                    Sha256Hash = this.persistedQueryHashes[BookSlotLockOperationName]
                }
            };

            string relativeUrl = $"{GraphQlRelativeUrl}?" +
                $"optype={MutationOperationType}&" +
                $"opname={BookSlotLockOperationName}";

            return await this.PostAync<ExternalOpenTableSlotLockBooking, ExternalOpenTableSlotLockBookingResult>(relativeUrl, externalOpenTableSlotLockBooking);
        }
    }
}
