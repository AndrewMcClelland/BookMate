// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using BookMate.Core.Api.Models.TeeTimes;
using BookMate.Core.Api.Models.TeeTimes.Exceptions;
using RESTFulSense.Exceptions;

namespace BookMate.Core.Api.Services.Foundations.ForeUpSoftware
{
    public partial class ForeUpSoftwareService
    {
        private delegate ValueTask<List<TeeTime>> ReturningTeeTimesFunction();

        private async ValueTask<List<TeeTime>> TryCatch(ReturningTeeTimesFunction returningTeeTimesFunction)
        {
            try
            {
                return await returningTeeTimesFunction();
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var failedTeeTimeDependencyException =
                    new FailedTeeTimeDependencyException(httpResponseUrlNotFoundException);

                throw CreateAndLogCriticalDependencyException(failedTeeTimeDependencyException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var failedTeeTimeDependencyException =
                    new FailedTeeTimeDependencyException(httpResponseUnauthorizedException);

                throw CreateAndLogCriticalDependencyException(failedTeeTimeDependencyException);
            }
            catch (HttpResponseForbiddenException httpResponseForbiddenException)
            {
                var failedTeeTimeDependencyException =
                    new FailedTeeTimeDependencyException(httpResponseForbiddenException);

                throw CreateAndLogCriticalDependencyException(failedTeeTimeDependencyException);
            }
        }

        private TeeTimeDependencyException CreateAndLogCriticalDependencyException(
            FailedTeeTimeDependencyException failedTeeTimeDependencyException)
        {
            var teeTimeDependencyException = new TeeTimeDependencyException(failedTeeTimeDependencyException);
            this.loggingBroker.LogCritical(teeTimeDependencyException);

            return teeTimeDependencyException;
        }
    }
}