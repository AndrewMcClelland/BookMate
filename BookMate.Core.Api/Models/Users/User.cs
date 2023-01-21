// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;
using System.Collections.Generic;

namespace BookMate.Core.Api.Models.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IDictionary<string, string> AuthTokens { get; set; }
    }
}
