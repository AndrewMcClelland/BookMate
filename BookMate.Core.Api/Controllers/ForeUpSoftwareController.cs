// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.TeeTimes;
using BookMate.Core.Api.Models.TeeTimes.ForeUpSoftware;
using BookMate.Core.Api.Services.Foundations.ForeUpSoftware;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace BookMate.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForeUpSoftwareController : RESTFulController
    {
        private readonly IForeUpSoftwareService foreUpSoftwareService;

        public ForeUpSoftwareController(IForeUpSoftwareService foreUpSoftwareService) =>
            this.foreUpSoftwareService = foreUpSoftwareService;

        [HttpPost]
        public async ValueTask<ActionResult<List<TeeTime>>> GetAvailableTeeTimes(ForeUpSoftwareTeeTimeSearchCriteria teeTimeSearchCriteria)
        {
            List<TeeTime> availableTeeTimes = await this.foreUpSoftwareService.RetrieveAllAvailableTeeTimesAsync(teeTimeSearchCriteria);

            return Ok(availableTeeTimes);
        }
    }
}
