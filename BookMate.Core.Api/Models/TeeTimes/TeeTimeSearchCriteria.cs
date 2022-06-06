// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;

namespace BookMate.Core.Api.Models.TeeTimes
{
    public class TeeTimeSearchCriteria
    {
        public string CourseId { get; set; }
        public DateTimeOffset Date { get; set; }
        public TeeTimeHoles Holes { get; set; }
        public TeeTimePlayers Players { get; set; }
    }
}
