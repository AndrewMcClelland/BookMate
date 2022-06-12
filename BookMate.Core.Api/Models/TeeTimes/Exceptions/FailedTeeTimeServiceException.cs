// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;
using Xeptions;

namespace BookMate.Core.Api.Models.TeeTimes.Exceptions
{
    public class FailedTeeTimeServiceException : Xeption
    {
        public FailedTeeTimeServiceException(Exception innerException)
            : base(message: "Failed tee time service error occurred, contact support.",
                  innerException)
        { }
    }
}
