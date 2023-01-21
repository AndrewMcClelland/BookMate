// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.OpenTable
{
    public class ExternalOpenTableSlotLockBookingResult
    {
        [JsonProperty("data")]
        public ExternalOpenTableSlotLockBookingResultData Data { get; set; }
    }

    public class ExternalOpenTableSlotLockBookingResultData
    {
        [JsonProperty("lockSlot")]
        public ExternalOpenTableSlotLockBookingResultLockedSlot LockedSlot { get; set; }
    }

    public class ExternalOpenTableSlotLockBookingResultLockedSlot
    {
        [JsonProperty("success")]
        public bool IsSuccess { get; set; }

        [JsonProperty("slotLock")]
        public ExternalOpenTableSlotLockBookingResultSlotLock SlotLock { get; set; }

        [JsonProperty("slotLockErrors")]
        public object SlotLockErrors { get; set; }
    }

    public class ExternalOpenTableSlotLockBookingResultSlotLock
    {
        [JsonProperty("slotLockId")]
        public int Id { get; set; }
    }

}
