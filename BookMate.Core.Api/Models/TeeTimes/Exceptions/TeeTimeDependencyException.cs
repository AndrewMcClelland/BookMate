// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Xeptions;

namespace BookMate.Core.Api.Models.TeeTimes.Exceptions
{
    public class TeeTimeDependencyException : Xeption
    {
        public TeeTimeDependencyException(Xeption innerException)
            : base(message: "Tee time dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
