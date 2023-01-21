// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;

namespace BookMate.Core.Api.Models.Schedules
{
    public class SchedulePreferredDateTime
    {
        public DateTimeOffset DateTime { get; set; }
        public int Rank { get; set; }
    }
}
