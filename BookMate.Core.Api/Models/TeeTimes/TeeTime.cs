// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;

namespace BookMate.Core.Api.Models.TeeTimes
{
    public class TeeTime
    {
        public DateTimeOffset DateTime { get; set; }
        public string CourseName { get; set; }
        public double Cost { get; set; }
        public int Holes { get; set; }
        public int Players { get; set; }
    }
}
