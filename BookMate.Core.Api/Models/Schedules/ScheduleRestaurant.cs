// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using BookMate.Core.Api.Models.Restaurants;
using BookMate.Core.Api.Models.Users;

namespace BookMate.Core.Api.Models.Schedules
{
    public class ScheduleRestaurant : Schedule
    {
        public ScheduleRestaurant() =>
            this.Type = ScheduleType.Restaurant;

        public Restaurant Restaurant { get; set; }
        public User User { get; set; }
        public List<SchedulePreferredDateTime> PreferredDateTimes { get; set; }
    }
}
