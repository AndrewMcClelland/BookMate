// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.ForeUpSoftware;

namespace BookMate.Core.Api.Brokers.BookingSystems.ForeUpSoftwareBookingSystems
{
    public partial class ForeUpSoftwareBookingSystemBroker : IForeUpSoftwareBookingSystemBroker
    {
        private const string BookingsRelativeUrl = "api/booking/times";

        public async ValueTask<List<ExternalForeUpSoftwareTeeTime>> GetAvailableTeeTimes(ExternalForeUpSoftwareBookingCriteria externalForeUpSoftwareBookingCriteria)
        {
            string relativeUrl = $"{BookingsRelativeUrl}?" +
                             $"time={externalForeUpSoftwareBookingCriteria.Time}&" +
                             $"date={externalForeUpSoftwareBookingCriteria.Date}&" +
                             $"holes={externalForeUpSoftwareBookingCriteria.Holes}&" +
                             $"players={externalForeUpSoftwareBookingCriteria.Players}&" +
                             $"schedule_id={externalForeUpSoftwareBookingCriteria.CourseId}&" +
                             $"specials_only={externalForeUpSoftwareBookingCriteria.SpecialsOnly}&" +
                             $"booking_class={externalForeUpSoftwareBookingCriteria.BookingClass}&" +
                             $"api_key=no_limits";

            return await this.GetAsync<List<ExternalForeUpSoftwareTeeTime>>(relativeUrl);
        }
    }
}