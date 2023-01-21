// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.OpenTable;

namespace BookMate.Core.Api.Brokers.APIs.OpenTable
{
    public partial interface IOpenTableApiBroker
    {
        ValueTask<ExternalOpenTableSlotLockBookingResult> BookRestaurantSlotLock(ExternalOpenTableSlotLockBooking externalOpenTableSlotLockBooking);
    }
}
