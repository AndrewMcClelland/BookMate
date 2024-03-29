﻿// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookMate.Core.Api.Brokers.BookingSystems.ForeUpSoftwareBookingSystems;
using BookMate.Core.Api.Brokers.Loggings;
using BookMate.Core.Api.Models.Externals.ForeUpSoftware;
using BookMate.Core.Api.Models.TeeTimes;
using BookMate.Core.Api.Models.TeeTimes.ForeUpSoftware;

namespace BookMate.Core.Api.Services.Foundations.ForeUpSoftware
{
    public partial class ForeUpSoftwareService : IForeUpSoftwareService
    {
        private readonly IForeUpSoftwareBookingSystemBroker foreUpSoftwareBookingSystemBroker;
        private readonly ILoggingBroker loggingBroker;

        public ForeUpSoftwareService(IForeUpSoftwareBookingSystemBroker foreUpSoftwareBookingSystemBroker, ILoggingBroker loggingBroker)
        {
            this.foreUpSoftwareBookingSystemBroker = foreUpSoftwareBookingSystemBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<TeeTime>> RetrieveAllAvailableTeeTimesAsync(ForeUpSoftwareTeeTimeSearchCriteria teeTimeSearchCriteria) =>
        TryCatch(async () =>
        {
            var externalForeUpSoftwareBookingCriteria = new ExternalForeUpSoftwareBookingCriteria
            {
                CourseId = teeTimeSearchCriteria.CourseId,
                Date = teeTimeSearchCriteria.Date.Date.ToString("MM-dd-yyyy"),
                Time = "all",
                Holes = GetForeUpSoftwareSearchHoles(teeTimeSearchCriteria),
                Players = GetForeUpSoftwareSearchPlayers(teeTimeSearchCriteria),
                BookingClass = teeTimeSearchCriteria.BookingClass,
                SpecialsOnly = "0",
            };

            List<ExternalForeUpSoftwareTeeTime> externalForeUpSoftwareTeeTimes =
                await this.foreUpSoftwareBookingSystemBroker.GetAvailableTeeTimes(externalForeUpSoftwareBookingCriteria);

            return externalForeUpSoftwareTeeTimes.Select(AsTeeTime).ToList();
        });

        private static string GetForeUpSoftwareSearchHoles(TeeTimeSearchCriteria teeTimeSearchCriteria)
        {
            return teeTimeSearchCriteria.Holes switch
            {
                TeeTimeHoles.Nine => "9",
                TeeTimeHoles.Eighteen => "18",
                _ => "all"
            };
        }

        private static string GetForeUpSoftwareSearchPlayers(TeeTimeSearchCriteria teeTimeSearchCriteria)
        {
            return teeTimeSearchCriteria.Players switch
            {
                TeeTimePlayers.One => "1",
                TeeTimePlayers.Two => "2",
                TeeTimePlayers.Three => "3",
                TeeTimePlayers.Four => "4",
                _ => "0"
            };
        }

        private static readonly Func<ExternalForeUpSoftwareTeeTime, TeeTime> AsTeeTime = externalForeUpSoftwareTeeTime =>
        {
            return new TeeTime
            {
                CourseName = externalForeUpSoftwareTeeTime.CourseName,
                DateTime = DateTimeOffset.Parse(externalForeUpSoftwareTeeTime.Time),
                Holes = GetForeUpSoftwareTeeTimeHoles(externalForeUpSoftwareTeeTime.Holes),
                Players = externalForeUpSoftwareTeeTime.AvailableSpots,
                Cost = externalForeUpSoftwareTeeTime.GreenFee,
            };
        };
        private static TeeTimeHoles GetForeUpSoftwareTeeTimeHoles(string holes)
        {
            return holes switch
            {
                "9" => TeeTimeHoles.Nine,
                "18" => TeeTimeHoles.Eighteen,
                _ => TeeTimeHoles.Any,
            };
        }
    }
}