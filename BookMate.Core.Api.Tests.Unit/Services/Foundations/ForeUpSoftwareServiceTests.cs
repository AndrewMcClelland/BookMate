// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Linq.Expressions;
using BookMate.Core.Api.Brokers.BookingSystems.ForeUpSoftwareBookingSystems;
using BookMate.Core.Api.Brokers.Loggings;
using BookMate.Core.Api.Models.Externals.ForeUpSoftware;
using BookMate.Core.Api.Models.TeeTimes;
using BookMate.Core.Api.Services.Foundations.ForeUpSoftware;
using KellermanSoftware.CompareNetObjects;
using Moq;
using RESTFulSense.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace BookMate.Core.Api.Tests.Unit.Services.Foundations
{
    public partial class ForeUpSoftwareServiceTests
    {
        private readonly Mock<IForeUpSoftwareBookingSystemBroker> foreUpSoftwareBookingSystemBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IForeUpSoftwareService foreUpSoftwareService;

        public ForeUpSoftwareServiceTests()
        {
            this.foreUpSoftwareBookingSystemBrokerMock = new Mock<IForeUpSoftwareBookingSystemBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.foreUpSoftwareService = new ForeUpSoftwareService(
                foreUpSoftwareBookingSystemBroker: this.foreUpSoftwareBookingSystemBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        public static TheoryData CriticalDependencyException()
        {
            string someMessage = GetRandomString();
            var someResponseMessage = new HttpResponseMessage();

            return new TheoryData<Xeption>()
            {
                new HttpResponseUrlNotFoundException(someResponseMessage, someMessage),
                new HttpResponseUnauthorizedException(someResponseMessage, someMessage),
                new HttpResponseForbiddenException(someResponseMessage, someMessage)
            };
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static List<dynamic> CreateRandomTeeTimeProperties()
        {
            return Enumerable.Range(start: 0, count: GetRandomNumber())
                .Select(item =>
                {
                    (DateTimeOffset time, string timeExternal) = GetRandomTeeTimeDateTime();
                    (TeeTimeHoles holes, string holesExteral) = GetRandomHoles();
                    int randomPlayers = GetRandomNumber(min: 1, max: 4);
                    double randomCost = GetRandomDouble();

                    return new
                    {
                        CourseName = GetRandomString(),
                        Time = time,
                        TimeExternal = timeExternal,
                        Holes = holes,
                        HolesExternal = holesExteral,
                        Players = randomPlayers,
                        AvailableSpots = randomPlayers,
                        Cost = randomCost,
                        GreenFee = randomCost
                    };
                }).ToList<dynamic>();
        }

        private static dynamic CreateRandomBookingCriteriaProperties()
        {
            (DateTimeOffset date, string dateExternal) = GetRandomTeeTimeSearchDateTime();
            (TeeTimeHoles holes, string holesExteral) = GetRandomHoles();
            (TeeTimePlayers players, string playersExternal) = GetRandomPlayers();

            return new
            {
                CourseId = GetRandomString(),
                Date = date,
                DateExternal = dateExternal,
                Holes = holes,
                HolesExternal = holesExteral,
                Players = players,
                PlayersExternal = playersExternal,
                BookingClass = GetRandomString()
            };
        }

        private static (DateTimeOffset, string) GetRandomTeeTimeDateTime()
        {
            var randomDateRaw = DateTimeOffset.Now.AddDays(new Random().Next(1000));

            var randomDate = new DateTimeOffset(
                randomDateRaw.Year,
                randomDateRaw.Month,
                randomDateRaw.Day,
                randomDateRaw.Hour,
                randomDateRaw.Minute,
                randomDateRaw.Second,
                randomDateRaw.Offset);

            var randomDateString = randomDate.ToString();

            return (randomDate, randomDateString);
        }

        private static (DateTimeOffset, string) GetRandomTeeTimeSearchDateTime()
        {
            var randomDate = DateTime.UtcNow.AddDays(new Random().Next(1000));
            var randomDateString = randomDate.ToString("MM-dd-yyyy");

            return (randomDate, randomDateString);
        }

        private static (TeeTimeHoles, string) GetRandomHoles()
        {
            var allHoles = new List<(TeeTimeHoles, string)>
            {
                (TeeTimeHoles.Nine, "9"),
                (TeeTimeHoles.Eighteen, "18"),
                (TeeTimeHoles.Any, "all"),
            };

            return GetRandomListItem(allHoles);
        }

        private static (TeeTimePlayers, string) GetRandomPlayers()
        {
            var allPlayers = new List<(TeeTimePlayers, string)>
            {
                (TeeTimePlayers.One, "1"),
                (TeeTimePlayers.Two, "2"),
                (TeeTimePlayers.Three, "3"),
                (TeeTimePlayers.Four, "4"),
                (TeeTimePlayers.Any, "0"),
            };

            return GetRandomListItem(allPlayers);
        }

        private static (T, U) GetRandomListItem<T, U>(List<(T, U)> list)
        {
            return list
                .OrderBy(item => new Random().Next())
                    .First();
        }

        private Expression<Func<ExternalForeUpSoftwareBookingCriteria, bool>> SameInformationAs(
            ExternalForeUpSoftwareBookingCriteria expectedExternalForeUpSoftwareBookingCriteria)
        {
            return actualExternalForeUpSoftwareBookingCriteria =>
                this.compareLogic.Compare(
                    expectedExternalForeUpSoftwareBookingCriteria,
                    actualExternalForeUpSoftwareBookingCriteria)
                        .AreEqual;
        }

        private static int GetRandomNumber(int min = 1, int max = 20) =>
            new IntRange(min, max + 1).GetValue();

        private static double GetRandomDouble(double min = 0.01, double max = 100.0) =>
            new DoubleRange(min, max + 1).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();
    }
}
