// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.TeeTimes;
using BookMate.Core.Api.Models.TeeTimes.ForeUpSoftware;

namespace BookMate.Core.Api.Services.Foundations.ForeUpSoftware
{
    public interface IForeUpSoftwareService
    {
        ValueTask<List<TeeTime>> RetrieveAllAvailableTeeTimesAsync(ForeUpSoftwareTeeTimeSearchCriteria teeTimeSearchCriteria);
    }
}
