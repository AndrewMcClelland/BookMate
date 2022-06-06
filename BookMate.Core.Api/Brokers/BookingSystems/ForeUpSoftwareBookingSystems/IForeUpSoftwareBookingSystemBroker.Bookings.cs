// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.Externals.ForeUpSoftware;

namespace BookMate.Core.Api.Brokers.BookingSystems.ForeUpSoftwareBookingSystems
{
    public partial interface IForeUpSoftwareBookingSystemBroker
    {
        ValueTask<List<ExternalForeUpSoftwareTeeTime>> GetAvailableTeeTimes(ExternalForeUpSoftwareBookingCriteria externalForeUpSoftwareBookingCriteria);
    }
}
