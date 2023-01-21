// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.Resy
{
    public class ExternalResyVenueSlotsSearchResult
    {
        [JsonProperty("results")]
        public ExternalResyVenueSlotsSearchResultResults Results { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultResults
    {
        [JsonProperty("venues")]
        public ExternalResyVenueSlotsSearchResultVenue[] Venues { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultVenue
    {
        [JsonProperty("venue")]
        public ExternalResyVenueSlotsSearchResultVenueDetails VenueDetails { get; set; }

        [JsonProperty("slots")]
        public ExternalResyVenueSlotsSearchResultSlot[] Slots { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultVenueDetails
    {
        [JsonProperty("id")]
        public ExternalResyVenueSlotsSearchResultVenueId Id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("url_slug")]
        public string url_slug { get; set; }

        [JsonProperty("price_range")]
        public int price_range { get; set; }

        [JsonProperty("average_bill_size")]
        public float average_bill_size { get; set; }

        [JsonProperty("currency_symbol")]
        public string currency_symbol { get; set; }

        [JsonProperty("transaction_processor")]
        public string transaction_processor { get; set; }

        [JsonProperty("rating")]
        public float rating { get; set; }

        [JsonProperty("total_ratings")]
        public int total_ratings { get; set; }

        [JsonProperty("location")]
        public ExternalResyVenueSlotsSearchResultVenueLocation Location { get; set; }

        [JsonProperty("travel_time")]
        public ExternalResyVenueSlotsSearchResultVenueTravelTime travel_time { get; set; }

        [JsonProperty("top")]
        public bool top { get; set; }

        [JsonProperty("ticket")]
        public ExternalResyVenueSlotsSearchResultVenueTicket ticket { get; set; }

        [JsonProperty("currency")]
        public ExternalResyVenueSlotsSearchResultVenueCurrency currency { get; set; }

        [JsonProperty("supports_pickups")]
        public int supports_pickups { get; set; }

        [JsonProperty("content")]
        public ExternalResyVenueSlotsSearchResultVenueContent[] content { get; set; }

        [JsonProperty("allow_bypass_payment_method")]
        public int allow_bypass_payment_method { get; set; }

        [JsonProperty("events")]
        public object[] events { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultVenueId
    {
        [JsonProperty("resy")]
        public int resy { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultVenueLocation
    {
        [JsonProperty("time_zone")]
        public string time_zone { get; set; }

        [JsonProperty("neighborhood")]
        public string neighborhood { get; set; }

        [JsonProperty("geo")]
        public ExternalResyVenueSlotsSearchResultVenueLocationGeo geo { get; set; }

        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultVenueLocationGeo
    {
        [JsonProperty("lat")]
        public float lat { get; set; }

        [JsonProperty("lon")]
        public float lon { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultVenueTravelTime
    {
        [JsonProperty("distance")]
        public float distance { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultVenueTicket
    {
        [JsonProperty("average")]
        public float average { get; set; }

        [JsonProperty("average_str")]
        public string AverageString { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultVenueCurrency
    {
        [JsonProperty("symbol")]
        public string symbol { get; set; }

        [JsonProperty("code")]
        public string code { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultVenueContent
    {
        [JsonProperty("body")]
        public string body { get; set; }

        [JsonProperty("locale")]
        public ExternalResyVenueSlotsSearchResultVenueContentLocale locale { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultVenueContentLocale
    {
        [JsonProperty("language")]
        public string language { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultSlot
    {
        [JsonProperty("config")]
        public ExternalResyVenueSlotsSearchResultSlotConfig config { get; set; }

        [JsonProperty("date")]
        public ExternalResyVenueSlotsSearchResultSlotDate date { get; set; }

        [JsonProperty("size")]
        public ExternalResyVenueSlotsSearchResultSlotPartySize PartySize { get; set; }

        [JsonProperty("payment")]
        public ExternalResyVenueSlotsSearchResultSlotPayment payment { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultSlotConfig
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("token")]
        public string token { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultSlotDate
    {
        [JsonProperty("end")]
        public string end { get; set; }

        [JsonProperty("start")]
        public string start { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultSlotPartySize
    {
        [JsonProperty("max")]
        public int max { get; set; }

        [JsonProperty("min")]
        public int min { get; set; }
    }

    public class ExternalResyVenueSlotsSearchResultSlotPayment
    {
        [JsonProperty("is_paid")]
        public bool is_paid { get; set; }

        [JsonProperty("cancellation_fee")]
        public float? cancellation_fee { get; set; }

        [JsonProperty("secs_cancel_cut_off")]
        public int? secs_cancel_cut_off { get; set; }

        [JsonProperty("time_cancel_cut_off")]
        public string time_cancel_cut_off { get; set; }

        [JsonProperty("secs_change_cut_off")]
        public int? secs_change_cut_off { get; set; }

        [JsonProperty("time_change_cut_off")]
        public string time_change_cut_off { get; set; }
    }
}
