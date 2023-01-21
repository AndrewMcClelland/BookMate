// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;

namespace BookMate.Core.Api.Models.Schedules
{
    public class Schedule
    {
        public Guid Id { get; set; }
        public ScheduleType Type { get; set; }
        public ScheduleSubType SubType { get; set; }
    }
}
