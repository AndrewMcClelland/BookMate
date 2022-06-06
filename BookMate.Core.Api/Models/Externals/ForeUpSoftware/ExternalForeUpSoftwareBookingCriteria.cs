// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

namespace BookMate.Core.Api.Models.Externals.ForeUpSoftware
{
    public class ExternalForeUpSoftwareBookingCriteria
    {
        public string CourseId { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public string Holes { get; set; }
        public string Players { get; set; }
        public string BookingClass { get; set; }
        public string SpecialsOnly { get; set; }
    }
}
