// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;

namespace BookMate.Core.Api.Models.Resy
{
    public class ResyRestaurantSlotDetailsSearch
    {
        public string ConfigId { get; set; }
        public DateTimeOffset Date { get; set; }
        public int PartySize { get; set; }
    }
}
