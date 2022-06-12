// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Xeptions;

namespace BookMate.Core.Api.Models.TeeTimes.Exceptions
{
    public class TeeTimeServiceException : Xeption
    {
        public TeeTimeServiceException(Xeption innerException)
            : base(message: "Tee time service error occurred, contact support.",
                  innerException)
        { }
    }
}
