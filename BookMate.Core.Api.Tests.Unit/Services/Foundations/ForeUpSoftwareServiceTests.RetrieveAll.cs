// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using BookMate.Core.Api.Models.Externals.ForeUpSoftware;
using BookMate.Core.Api.Models.TeeTimes;
using BookMate.Core.Api.Models.TeeTimes.ForeUpSoftware;
using FluentAssertions;
using Moq;
using Xunit;

namespace BookMate.Core.Api.Tests.Unit.Services.Foundations
{
    public partial class ForeUpSoftwareServiceTests
    {
        [Fact]
        public async Task RetrieveAllAvailableTeeTimesAsync()
        {
            // given
            List<dynamic> randomTeeTimeProperties = CreateRandomTeeTimeProperties();

            List<ExternalForeUpSoftwareTeeTime> randomExternalForeUpSoftwareTeeTime = 
                randomTeeTimeProperties.Select(item =>
                {
                    return new ExternalForeUpSoftwareTeeTime
                    {
                        CourseName = item.CourseName,
                        Time = item.TimeExternal,
                        Holes = item.HolesExternal,
                        AvailableSpots = item.AvailableSpots,
                        GreenFee = item.GreenFee
                    };
                }).ToList();


            List<TeeTime> randomTeeTimes = randomTeeTimeProperties.Select(item =>
            {
                return new TeeTime
                {
                    CourseName = item.CourseName,
                    DateTime = item.Time,
                    Holes = item.Holes,
                    Players = item.AvailableSpots,
                    Cost = item.GreenFee
                };
            }).ToList();

            List<ExternalForeUpSoftwareTeeTime> retrievedExternalForeUpSoftwareTeeTimes = 
                randomExternalForeUpSoftwareTeeTime;

            List<TeeTime> expectedTeeTimes = randomTeeTimes;

            dynamic randomBookingCriteriaProperties = CreateRandomBookingCriteriaProperties();
            
            var foreUpSoftwareBookingCriteria = new ForeUpSoftwareTeeTimeSearchCriteria
            {
                CourseId = randomBookingCriteriaProperties.CourseId,
                Date = randomBookingCriteriaProperties.Date,
                Holes = randomBookingCriteriaProperties.Holes,
                Players = randomBookingCriteriaProperties.Players,
                BookingClass = randomBookingCriteriaProperties.BookingClass,
            };

            var externalForeUpSoftwareBookingCriteria = new ExternalForeUpSoftwareBookingCriteria
            {
                CourseId = randomBookingCriteriaProperties.CourseId,
                Time = "all",
                Date = randomBookingCriteriaProperties.DateExternal,
                Holes = randomBookingCriteriaProperties.HolesExternal,
                Players = randomBookingCriteriaProperties.PlayersExternal,
                BookingClass = randomBookingCriteriaProperties.BookingClass,
                SpecialsOnly = "0"
            };

            this.foreUpSoftwareBookingSystemBrokerMock.Setup(broker =>
                broker.GetAvailableTeeTimes(It.Is(
                    SameInformationAs(externalForeUpSoftwareBookingCriteria))))
                        .ReturnsAsync(retrievedExternalForeUpSoftwareTeeTimes);

            // when
            List<TeeTime> actualTeeTimes = 
                await this.foreUpSoftwareService.RetrieveAllAvailableTeeTimesAsync(foreUpSoftwareBookingCriteria);

            // then
            actualTeeTimes.Should().BeEquivalentTo(expectedTeeTimes);

            this.foreUpSoftwareBookingSystemBrokerMock.Verify(broker =>
                broker.GetAvailableTeeTimes(It.Is(
                    SameInformationAs(externalForeUpSoftwareBookingCriteria))),
                        Times.Once);

            this.foreUpSoftwareBookingSystemBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
