// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;

namespace BookMate.Core.Api.Models.Resy
{
    public class ResyRestaurantSlotDetails
    {
        public string BookingToken { get; set; }
        public DateTimeOffset? BookingTokenExpirationDateTime { get; set; }
        public float? CancellationFee { get; set; }
        public string CancellationPolicy { get; set; }
        public DateTimeOffset? CancelCutOffDateTime { get; set; }
        public DateTimeOffset? ChangeCutOffDateTime { get; set; }
        public string PaymentMethodId { get; set; }
        public string Currency { get; set; }
        public int? PartySize { get; set; }
        public float? ReservationFee { get; set; }
        public float? ResyFee { get; set; }
        public float? ServiceFee { get; set; }
        public float? Tax { get; set; }
        public float? TotalFee { get; set; }
        public ResyRating Rating { get; set; }
    }
}
