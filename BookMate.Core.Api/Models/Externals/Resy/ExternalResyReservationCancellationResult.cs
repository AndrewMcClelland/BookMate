// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.Resy
{
    public class ExternalResyReservationCancellationResult
    {
        [JsonProperty("payment")]
        public ExternalResyReservationCancellationResultPayment Payment { get; set; }
    }

    public class ExternalResyReservationCancellationResultPayment
    {
        [JsonProperty("transaction")]
        public ExternalResyReservationCancellationResultTransaction Transaction { get; set; }
    }

    public class ExternalResyReservationCancellationResultTransaction
    {
        [JsonProperty("refund")]
        public int Refund { get; set; }
    }

}
