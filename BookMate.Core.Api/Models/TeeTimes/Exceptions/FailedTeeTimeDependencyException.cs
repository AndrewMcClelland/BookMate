// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;
using Xeptions;

namespace BookMate.Core.Api.Models.TeeTimes.Exceptions
{
    public class FailedTeeTimeDependencyException : Xeption
    {
        public FailedTeeTimeDependencyException(Exception innerException)
            : base(message: "Failed tee time dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
