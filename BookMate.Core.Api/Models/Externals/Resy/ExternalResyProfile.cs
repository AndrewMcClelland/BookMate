// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.Resy
{
    public class ExternalResyProfile
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("mobile_number")]
        public string MobileNumber { get; set; }

        [JsonProperty("em_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("payment_methods")]
        public ExternalResyProfilePaymentMethod[] PaymentMethods { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }

        [JsonProperty("payment_method_id")]
        public int PaymentMethodId { get; set; }

        [JsonProperty("payment_provider_id")]
        public int PaymentProviderId { get; set; }

        [JsonProperty("payment_provider_name")]
        public string PaymentProviderName { get; set; }

        [JsonProperty("payment_display")]
        public string PaymentDisplay { get; set; }

        [JsonProperty("guest_id")]
        public int GuestId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("legacy_token")]
        public string LegacyToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }

    public class ExternalResyProfilePaymentMethod
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("provider_id")]
        public int ProviderId { get; set; }

        [JsonProperty("provider_name")]
        public string ProviderName { get; set; }

        [JsonProperty("display")]
        public string DisplayedCardDigits { get; set; }

        [JsonProperty("type")]
        public string CardType { get; set; }

        [JsonProperty("exp_year")]
        public int ExpirationYear { get; set; }

        [JsonProperty("exp_month")]
        public int ExpirationMonth { get; set; }

        [JsonProperty("issuing_bank")]
        public string IssuingBank { get; set; }
    }

}
