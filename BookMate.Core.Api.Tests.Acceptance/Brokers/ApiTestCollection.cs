// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Xunit;

namespace BookMate.Core.Api.Tests.Acceptance.Brokers
{
    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<BookMateApiBroker>
    {
    }
}
