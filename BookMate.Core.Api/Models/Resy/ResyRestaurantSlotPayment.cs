// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

namespace BookMate.Core.Api.Models.Resy
{
    public class ResyRestaurantSlotPayment
    {
        public bool IsPaid { get; set; }
        public float? CancellationFee { get; set; }
        public int? CancelCutOffSeconds { get; set; }
        public string CancelCutOffTime { get; set; }
        public int? ChangeCutOffSeconds { get; set; }
        public string ChangeCutOffTime { get; set; }
    }
}