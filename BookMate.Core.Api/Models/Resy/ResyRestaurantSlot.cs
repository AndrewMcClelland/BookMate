// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;

namespace BookMate.Core.Api.Models.Resy
{
    public class ResyRestaurantSlot
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Token { get; set; }
        public int PartySize { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public ResyRestaurantSlotPayment Payment { get; set; }
    }
}
