// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using BookMate.Core.Api.Tests.Acceptance.Brokers;
using FluentAssertions;
using Xunit;

namespace BookMate.Core.Api.Tests.Acceptance.APIs.Homes
{
    [Collection(nameof(ApiTestCollection))]
    public class HomeApiTests
    {
        private readonly BookMateApiBroker bookMateApiBroker;

        public HomeApiTests(BookMateApiBroker bookMateApiBroker) => this.bookMateApiBroker = bookMateApiBroker;

        [Fact]
        public async Task ShouldReturnHomeMessage()
        {
            // given
            string expectedMessage = "Hello world!";

            // when
            string actualMessage = await bookMateApiBroker.GetHomeMessageAsync();

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);
        }
    }
}
