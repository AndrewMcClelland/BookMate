// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using Newtonsoft.Json;

namespace BookMate.Core.Api.Models.Externals.ForeUpSoftware
{
    public class ExternalForeUpSoftwareTeeTime
    {
        [JsonProperty("time")]
        public string Time { get; set; }
        [JsonProperty("start_front")]
        public long StartFront { get; set; }
        [JsonProperty("course_id")]
        public int CourseId { get; set; }
        [JsonProperty("course_name")]
        public string CourseName { get; set; }
        [JsonProperty("schedule_id")]
        public int ScheduleId { get; set; }
        [JsonProperty("teesheet_id")]
        public int TeesheetId { get; set; }
        [JsonProperty("schedule_name")]
        public string ScheduleName { get; set; }
        [JsonProperty("require_credit_card")]
        public bool RequireCreditCard { get; set; }
        [JsonProperty("teesheet_holes")]
        public int TeesheetHoles { get; set; }
        [JsonProperty("teesheet_side_id")]
        public int TeesheetSideId { get; set; }
        [JsonProperty("teesheet_side_name")]
        public string TeesheetSideName { get; set; }
        [JsonProperty("teesheet_side_order")]
        public int TeesheetSideOrder { get; set; }
        [JsonProperty("reround_teesheet_side_id")]
        public int ReroundTeesheetSideId { get; set; }
        [JsonProperty("reround_teesheet_side_name")]
        public string ReroundTeesheetSideName { get; set; }
        [JsonProperty("available_spots")]
        public int AvailableSpots { get; set; }
        [JsonProperty("available_spots_9")]
        public int AvailableSpots9 { get; set; }
        [JsonProperty("available_spots_18")]
        public int AvailableSpots18 { get; set; }
        [JsonProperty("maximum_players_per_booking")]
        public string MaximumPlayersPerBooking { get; set; }
        [JsonProperty("minimum_players")]
        public string MinimumPlayers { get; set; }
        [JsonProperty("allowed_group_sizes")]
        public string[] AllowedGroupSizes { get; set; }
        [JsonProperty("holes")]
        public string Holes { get; set; }
        [JsonProperty("has_special")]
        public bool HasSpecial { get; set; }
        [JsonProperty("special_id")]
        public bool SpecialId { get; set; }
        [JsonProperty("special_discount_percentage")]
        public int SpecialDiscountPercentage { get; set; }
        [JsonProperty("group_id")]
        public bool GroupId { get; set; }
        [JsonProperty("booking_class_id")]
        public int BookingClassId { get; set; }
        [JsonProperty("booking_fee_required")]
        public bool BookingFeeRequired { get; set; }
        [JsonProperty("booking_fee_price")]
        public bool BookingFeePrice { get; set; }
        [JsonProperty("booking_fee_per_person")]
        public bool BookingFeePerPerson { get; set; }
        [JsonProperty("foreup_trade_discount_rate")]
        public int ForeupTradeDiscountRate { get; set; }
        [JsonProperty("trade_min_players")]
        public int TradeMinPlayers { get; set; }
        [JsonProperty("trade_available_players")]
        public int TradeAvailablePlayers { get; set; }
        [JsonProperty("green_fee_tax_rate")]
        public bool GreenFeeTaxRate { get; set; }
        [JsonProperty("green_fee_tax")]
        public int GreenFeeTax { get; set; }
        [JsonProperty("green_fee_tax_9")]
        public int GreenFeeTax9 { get; set; }
        [JsonProperty("green_fee_tax_18")]
        public int GreenFeeTax18 { get; set; }
        [JsonProperty("guest_green_fee_tax_rate")]
        public bool GuestGreenFeeTaxRate { get; set; }
        [JsonProperty("guest_green_fee_tax")]
        public int GuestGreenFeeTax { get; set; }
        [JsonProperty("guest_green_fee_tax_9")]
        public int GuestGreenFeeTax9 { get; set; }
        [JsonProperty("guest_green_fee_tax_18")]
        public int GuestGreenFeeTax18 { get; set; }
        [JsonProperty("cart_fee_tax_rate")]
        public bool CartFeeTaxRate { get; set; }
        [JsonProperty("cart_fee_tax")]
        public int CartFeeTax { get; set; }
        [JsonProperty("cart_fee_tax_9")]
        public int CartFeeTax9 { get; set; }
        [JsonProperty("cart_fee_tax_18")]
        public int CartFeeTax18 { get; set; }
        [JsonProperty("guest_cart_fee_tax_rate")]
        public bool GuestCartFeeTaxRate { get; set; }
        [JsonProperty("guest_cart_fee_tax")]
        public int GuestCartFeeTax { get; set; }
        [JsonProperty("guest_cart_fee_tax_9")]
        public int GuestCartFeeTax9 { get; set; }
        [JsonProperty("guest_cart_fee_tax_18")]
        public int GuestCartFeeTax18 { get; set; }
        [JsonProperty("foreup_discount")]
        public bool ForeupDiscount { get; set; }
        [JsonProperty("pay_online")]
        public string PayOnline { get; set; }
        [JsonProperty("green_fee")]
        public double GreenFee { get; set; }
        [JsonProperty("green_fee_9")]
        public double GreenFee9 { get; set; }
        [JsonProperty("green_fee_18")]
        public double GreenFee18 { get; set; }
        [JsonProperty("guest_green_fee")]
        public double GuestGreenFee { get; set; }
        [JsonProperty("guest_green_fee_9")]
        public double GuestGreenFee9 { get; set; }
        [JsonProperty("guest_green_fee_18")]
        public double GuestGreenFee18 { get; set; }
        [JsonProperty("cart_fee")]
        public double CartFee { get; set; }
        [JsonProperty("cart_fee_9")]
        public double CartFee9 { get; set; }
        [JsonProperty("cart_fee_18")]
        public double CartFee18 { get; set; }
        [JsonProperty("guest_cart_fee")]
        public double GuestCartFee { get; set; }
        [JsonProperty("guest_cart_fee_9")]
        public double GuestCartFee9 { get; set; }
        [JsonProperty("guest_cart_fee_18")]
        public double GuestCartFee18 { get; set; }
        [JsonProperty("rate_type")]
        public string RateType { get; set; }
    }
}
