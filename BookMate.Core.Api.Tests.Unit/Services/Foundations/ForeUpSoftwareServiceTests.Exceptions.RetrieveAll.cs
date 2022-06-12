// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using BookMate.Core.Api.Models.Externals.ForeUpSoftware;
using BookMate.Core.Api.Models.TeeTimes;
using BookMate.Core.Api.Models.TeeTimes.Exceptions;
using BookMate.Core.Api.Models.TeeTimes.ForeUpSoftware;
using Moq;
using RESTFulSense.Exceptions;
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
                broker.GetAvailableTeeTimes(
                    It.IsAny<ExternalForeUpSoftwareBookingCriteria>()))
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

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveIfErrorOccursAndLogItAsync()
        {
            // given
            var someResponseMessage = new HttpResponseMessage();
            string someMessage = GetRandomString();
            var httpResponseException = new HttpResponseException(someResponseMessage, someMessage);

            var failedTeeTimeDependencyException =
                new FailedTeeTimeDependencyException(httpResponseException);

            var expectedTeeTimeDependencyException =
                new TeeTimeDependencyException(failedTeeTimeDependencyException);

            this.foreUpSoftwareBookingSystemBrokerMock.Setup(broker =>
                broker.GetAvailableTeeTimes(
                    It.IsAny<ExternalForeUpSoftwareBookingCriteria>()))
                        .ThrowsAsync(httpResponseException);

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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeeTimeDependencyException))),
                        Times.Once);

            this.foreUpSoftwareBookingSystemBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveIfErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedTeeTimeServiceException =
                new FailedTeeTimeServiceException(serviceException);

            var expectedTeeTimeServiceException =
                new TeeTimeServiceException(failedTeeTimeServiceException);

            this.foreUpSoftwareBookingSystemBrokerMock.Setup(broker =>
                broker.GetAvailableTeeTimes(
                    It.IsAny<ExternalForeUpSoftwareBookingCriteria>()))
                        .ThrowsAsync(serviceException);

            // when
            ValueTask<List<TeeTime>> retrieveAllTeeTimes =
                this.foreUpSoftwareService.RetrieveAllAvailableTeeTimesAsync(
                    new ForeUpSoftwareTeeTimeSearchCriteria());

            // then
            await Assert.ThrowsAsync<TeeTimeServiceException>(() =>
                retrieveAllTeeTimes.AsTask());

            this.foreUpSoftwareBookingSystemBrokerMock.Verify(broker =>
                broker.GetAvailableTeeTimes(
                    It.IsAny<ExternalForeUpSoftwareBookingCriteria>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeeTimeServiceException))),
                        Times.Once);

            this.foreUpSoftwareBookingSystemBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
