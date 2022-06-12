// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using BookMate.Core.Api.Models.Externals.ForeUpSoftware;
using BookMate.Core.Api.Models.TeeTimes;
using BookMate.Core.Api.Models.TeeTimes.Exceptions;
using BookMate.Core.Api.Models.TeeTimes.ForeUpSoftware;
using Moq;
using Xeptions;
using Xunit;

namespace BookMate.Core.Api.Tests.Unit.Services.Foundations
{
    public partial class ForeUpSoftwareServiceTests
    {
        [Theory]
        [MemberData(nameof(CriticalDependencyException))]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrievalIfCriticalErrorOccursAndLogItAsync(
            Xeption criticalDependencyException)
        {
            // given
            var failedTeeTimeDependencyException =
                new FailedTeeTimeDependencyException(criticalDependencyException);

            var expectedTeeTimeDependencyException =
                new TeeTimeDependencyException(failedTeeTimeDependencyException);

            this.foreUpSoftwareBookingSystemBrokerMock.Setup(broker =>
                broker.GetAvailableTeeTimes(It.IsAny<ExternalForeUpSoftwareBookingCriteria>()))
                    .ThrowsAsync(criticalDependencyException);

            // when
            ValueTask<List<TeeTime>> retrieveAllTeeTimes =
                this.foreUpSoftwareService.RetrieveAllAvailableTeeTimesAsync(
                    new ForeUpSoftwareTeeTimeSearchCriteria());

            // then
            await Assert.ThrowsAsync<TeeTimeDependencyException>(() =>
                retrieveAllTeeTimes.AsTask());

            this.foreUpSoftwareBookingSystemBrokerMock.Verify(broker =>
                broker.GetAvailableTeeTimes(
                    It.IsAny<ExternalForeUpSoftwareBookingCriteria>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTeeTimeDependencyException))),
                        Times.Once);

            this.foreUpSoftwareBookingSystemBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
