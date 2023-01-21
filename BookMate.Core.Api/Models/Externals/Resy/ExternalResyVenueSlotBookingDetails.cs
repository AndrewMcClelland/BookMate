using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.Resy
{
    public class ExternalResyVenueSlotBookingDetails
    {
        [JsonProperty("book_token")]
        public string BookingToken { get; set; }

        [JsonProperty("struct_payment_method")]
        public string PaymentMethod { get; set; }
    }
}
