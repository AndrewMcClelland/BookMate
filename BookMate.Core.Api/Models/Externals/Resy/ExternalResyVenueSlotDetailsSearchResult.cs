// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;
using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.Resy
{
    public class ExternalResyVenueSlotDetailsSearchResult
    {
        [JsonProperty("cancellation")]
        public ExternalResyVenueSlotDetailsSearchResultCancellationDetails CancellationDetails { get; set; }

        [JsonProperty("change")]
        public ExternalResyVenueSlotDetailsSearchResultChange change { get; set; }

        [JsonProperty("locale")]
        public ExternalResyVenueSlotDetailsSearchResultLocale locale { get; set; }

        [JsonProperty("payment")]
        public ExternalResyVenueSlotDetailsSearchResultPayment payment { get; set; }

        [JsonProperty("user")]
        public ExternalResyVenueSlotDetailsSearchResultUser user { get; set; }

        [JsonProperty("venue")]
        public ExternalResyVenueSlotDetailsSearchResultVenue venue { get; set; }

        [JsonProperty("book_token")]
        public ExternalResyVenueSlotDetailsSearchResultBookToken book_token { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultCancellationDetails
    {
        [JsonProperty("display")]
        public ExternalResyVenueSlotDetailsSearchResultDisplay Display { get; set; }

        [JsonProperty("fee")]
        public ExternalResyVenueSlotDetailsSearchResultFee Fee { get; set; }

        [JsonProperty("refund")]
        public ExternalResyVenueSlotDetailsSearchResultRefund Refund { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultDisplay
    {
        [JsonProperty("policy")]
        public string[] Policy { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultFee
    {
        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("display")]
        public ExternalResyVenueSlotDetailsSearchResultDisplayString DisplayString { get; set; }

        [JsonProperty("date_cut_off")]
        public DateTime DateCutOff { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultDisplayString
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultRefund
    {
        [JsonProperty("date_cut_off")]
        public DateTime DateCutOff { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultChange
    {
        [JsonProperty("date_cut_off")]
        public DateTime DateCutOff { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultLocale
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultPayment
    {
        [JsonProperty("amounts")]
        public ExternalResyVenueSlotDetailsSearchResultAmounts amounts { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultAmounts
    {
        [JsonProperty("reservation_charge")]
        public int reservation_charge { get; set; }

        [JsonProperty("subtotal")]
        public int subtotal { get; set; }

        [JsonProperty("add_ons")]
        public int add_ons { get; set; }

        [JsonProperty("quantity")]
        public int PartySize { get; set; }

        [JsonProperty("resy_fee")]
        public int resy_fee { get; set; }

        [JsonProperty("service_fee")]
        public int service_fee { get; set; }

        [JsonProperty("service_charge")]
        public ExternalResyVenueSlotDetailsSearchResultServiceCharge ServiceCharge { get; set; }

        [JsonProperty("tax")]
        public float tax { get; set; }

        [JsonProperty("total")]
        public float total { get; set; }

        [JsonProperty("surcharge")]
        public float surcharge { get; set; }

        [JsonProperty("price_per_unit")]
        public float price_per_unit { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultServiceCharge
    {
        [JsonProperty("amount")]
        public int amount { get; set; }

        [JsonProperty("value")]
        public string value { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultUser
    {
        [JsonProperty("payment_methods")]
        public ExternalResyProfilePaymentMethod[] PaymentMethods { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultVenue
    {
        [JsonProperty("location")]
        public ExternalResyVenueSlotDetailsSearchResultLocation Location { get; set; }

        [JsonProperty("rater")]
        public ExternalResyVenueSlotDetailsSearchResultRater[] rater { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultLocation
    {
        [JsonProperty("address_1")]
        public string address_1 { get; set; }

        [JsonProperty("address_2")]
        public object address_2 { get; set; }

        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }

        [JsonProperty("cross_street_1")]
        public string cross_street_1 { get; set; }

        [JsonProperty("cross_street_2")]
        public string cross_street_2 { get; set; }

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("latitude")]
        public float latitude { get; set; }

        [JsonProperty("locality")]
        public string locality { get; set; }

        [JsonProperty("longitude")]
        public float longitude { get; set; }

        [JsonProperty("neighborhood")]
        public string neighborhood { get; set; }

        [JsonProperty("postal_code")]
        public string postal_code { get; set; }

        [JsonProperty("region")]
        public string region { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultRater
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("scale")]
        public int scale { get; set; }

        [JsonProperty("score")]
        public float score { get; set; }

        [JsonProperty("total")]
        public int total { get; set; }
    }

    public class ExternalResyVenueSlotDetailsSearchResultBookToken
    {
        [JsonProperty("date_expires")]
        public DateTime date_expires { get; set; }

        [JsonProperty("value")]
        public string value { get; set; }
    }
}
