// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BookMate.Core.Api.Brokers.APIs.Resy;
using BookMate.Core.Api.Brokers.Loggings;
using BookMate.Core.Api.Models.Externals.Resy;
using BookMate.Core.Api.Models.Restaurants;
using BookMate.Core.Api.Models.Resy;

namespace BookMate.Core.Api.Services.Foundations.Resy
{
    public partial class ResyService : IResyService
    {
        private readonly IResyApiBroker resyApiBroker;
        private readonly ILoggingBroker loggingBroker;

        public ResyService(
            IResyApiBroker resyApiBroker,
            ILoggingBroker loggingBroker)
        {
            this.resyApiBroker = resyApiBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<ExternalResyProfile> AuthenticateWithCredentialsAsync(string email, string password)
        {
            var externalResyCredentials = new ExternalResyCredentials
            {
                Email = email,
                Password = password
            };

            return await this.resyApiBroker.AuthenticateCredentials(externalResyCredentials);
        }

        public async ValueTask<List<Restaurant>> SearchRestaurantsAsync(RestaurantSearchCriteria restaurantSearchCriteria, string resyToken = "")
        {
            var externalResyVenueSearch = new ExternalResyVenueSearch
            {
                Query = restaurantSearchCriteria.Name,
                NumberResults = 20,

                ResyVenueCoordinates = new ExternalResyVenueCoordinates
                {
                    Latitude = restaurantSearchCriteria.Coordinates.Latitude,
                    Longitude = restaurantSearchCriteria.Coordinates.Longitude
                },

                VenueTypes = new string[]
                {
                    "venue",
                    "cuisine"
                }
            };

            ExternalResyVenueSearchResult externalResyVenueSearchResult = await this.resyApiBroker.SearchAllVenues(externalResyVenueSearch, resyToken);

            List<Restaurant> restaurants = externalResyVenueSearchResult?.Search?.Results?.Select(searchResult =>
                new Restaurant
                {
                    BookingSystem = RestaurantBookingSystem.Resy,
                    Id = searchResult.Id.Resy.ToString(),
                    Name = searchResult.Name,
                    Cuisines = searchResult.Cuisine?.ToList(),
                    PriceRange = (RestaurantPriceRange)searchResult.PriceRangeId,

                    ContactInformation = new RestaurantContactInformation
                    {
                        PhoneNumber = searchResult.ContactInformation.PhoneNumber
                    },

                    Rating = new RestaurantRating
                    {
                        Average = searchResult.Rating.Average,
                        Count = searchResult.Rating.Count
                    },

                    Location = new RestaurantLocation
                    {
                        Neighborhood = searchResult.Neighborhood,
                        City = searchResult.Location.Name,
                        State = searchResult.Region,
                        Country = searchResult.Country,
                        Coordinates = new RestaurantCoordinates
                        {
                            Latitude = searchResult.Coordinates.Latitude,
                            Longitude = searchResult.Coordinates.Longitude
                        }
                    }
                }
            ).ToList();

            return restaurants;
        }

        public async ValueTask<List<ResyRestaurantSlot>> SearchRestaurantAvailabilityAsync(RestaurantReservationCriteria restaurantReservationCriteria, string resyToken = "")
        {
            var externalResyVenueSlotsSearch = new ExternalResyVenueSlotsSearch
            {
                Id = restaurantReservationCriteria.RestaurantId,
                Date = restaurantReservationCriteria.DateTime.ToString("yyyy-MM-dd"),
                PartySize = restaurantReservationCriteria.PartySize.ToString(),
                Latitude = restaurantReservationCriteria.RestaurantCoordinates.Latitude.ToString(),
                Longitude = restaurantReservationCriteria.RestaurantCoordinates.Longitude.ToString()
            };

            ExternalResyVenueSlotsSearchResult externalResyVenueSlotsSearchResult = await this.resyApiBroker.FindVenueSlots(externalResyVenueSlotsSearch, resyToken);

            List<ResyRestaurantSlot> resyRestaurantSlots = externalResyVenueSlotsSearchResult.Results.Venues.First().Slots.Select(slot =>
            new ResyRestaurantSlot
            {
                Id = slot.config.id.ToString(),
                Type = slot.config.type,
                Token = slot.config.token,
                PartySize = slot.PartySize.max,
                StartDateTime = DateTimeOffset.ParseExact(slot.date.start, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                EndDateTime = DateTimeOffset.ParseExact(slot.date.end, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                Payment = new ResyRestaurantSlotPayment
                {
                    IsPaid = slot.payment.is_paid,
                    CancellationFee = slot.payment.cancellation_fee,
                    CancelCutOffSeconds = slot.payment.secs_cancel_cut_off,
                    CancelCutOffTime = slot.payment.time_cancel_cut_off,
                    ChangeCutOffSeconds = slot.payment.secs_change_cut_off,
                    ChangeCutOffTime = slot.payment.time_change_cut_off
                }
            }).ToList();

            return resyRestaurantSlots;
        }

        public async ValueTask<ResyRestaurantSlotDetails> GetRestaurantSlotDetailsAsync(ResyRestaurantSlotDetailsSearch resyRestaurantSlotDetailsSearch, string resyToken = "")
        {
            var resyVenueSlotDetailsSearch = new ExternalResyVenueSlotDetailsSearch
            {
                ConfigId = resyRestaurantSlotDetailsSearch.ConfigId,
                Date = resyRestaurantSlotDetailsSearch.Date.ToString("yyyy-MM-dd"),
                PartySize = resyRestaurantSlotDetailsSearch.PartySize.ToString()
            };

            ExternalResyVenueSlotDetailsSearchResult externalResyVenueSlotDetailsSearchResult = await this.resyApiBroker.GetVenueSlotDetails(resyVenueSlotDetailsSearch, resyToken);

            var resyRestaurantSlotDetails = new ResyRestaurantSlotDetails
            {
                BookingToken = externalResyVenueSlotDetailsSearchResult.book_token.value,
                BookingTokenExpirationDateTime = externalResyVenueSlotDetailsSearchResult.book_token.date_expires,
                CancellationFee = externalResyVenueSlotDetailsSearchResult.CancellationDetails?.Fee?.Amount,
                CancellationPolicy = externalResyVenueSlotDetailsSearchResult.CancellationDetails?.Display?.Policy?.First(),
                CancelCutOffDateTime = externalResyVenueSlotDetailsSearchResult.CancellationDetails?.Fee?.DateCutOff,
                ChangeCutOffDateTime = externalResyVenueSlotDetailsSearchResult.change?.DateCutOff,
                PaymentMethodId = externalResyVenueSlotDetailsSearchResult.user?.PaymentMethods?.First()?.Id.ToString(),
                Currency = externalResyVenueSlotDetailsSearchResult.locale?.Currency,
                PartySize = externalResyVenueSlotDetailsSearchResult.payment?.amounts?.PartySize,
                ReservationFee = externalResyVenueSlotDetailsSearchResult.payment?.amounts?.reservation_charge,
                ResyFee = externalResyVenueSlotDetailsSearchResult.payment?.amounts?.resy_fee,
                ServiceFee = externalResyVenueSlotDetailsSearchResult.payment?.amounts?.service_fee,
                Tax = externalResyVenueSlotDetailsSearchResult.payment?.amounts?.tax,
                TotalFee = externalResyVenueSlotDetailsSearchResult.payment?.amounts?.total,
                Rating = new ResyRating
                {
                    Scale = externalResyVenueSlotDetailsSearchResult.venue?.rater?.First()?.scale,
                    Score = externalResyVenueSlotDetailsSearchResult.venue?.rater?.First()?.score,
                    Total = externalResyVenueSlotDetailsSearchResult.venue?.rater?.First()?.total
                }
            };

            return resyRestaurantSlotDetails;
        }

        public async ValueTask<ResyRestaurantSlotBooking> BookRestaurantSlotAsync(ResyRestaurantSlotBookingCriteria resyRestaurantSlotBookingCriteria, string resyToken = "")
        {
            var externalResyVenueSlotBookingDetails = new ExternalResyVenueSlotBookingDetails
            {
                BookingToken = resyRestaurantSlotBookingCriteria.BookingToken,
                PaymentMethod = $"{{\"id\":{resyRestaurantSlotBookingCriteria.PaymentMethodId}}}",
            };

            ExternalResyVenueSlotBookingResult externalResyVenueSlotBookingResult = await this.resyApiBroker.BookVenueSlot(externalResyVenueSlotBookingDetails, resyToken);

            var resyRestaurantSlotBooking = new ResyRestaurantSlotBooking
            {
                ReservationId = externalResyVenueSlotBookingResult.ReservationId.ToString(),
                Token = externalResyVenueSlotBookingResult.Token
            };

            return resyRestaurantSlotBooking;
        }

        //public async ValueTask<ExternalResyReservationCancellationResult> CancelReservationAsync(string reservationToken, string resyToken = "") =>
        //    await this.resyApiBroker.CancelReservation(reservationToken, resyToken);
    }
}
