// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookMate.Core.Api.Brokers.Loggings;
using BookMate.Core.Api.Models.Externals.OpenTable;
using BookMate.Core.Api.Models.Restaurants;
using BookMate.Core.Api.Services.Foundations.OpenTable;

namespace BookMate.Core.Api.Services.Orchestrations.OpenTable
{
    public class OpenTableOrchestrationService : IOpenTableOrchestrationService
    {
        private readonly IOpenTableService openTableService;
        private readonly ILoggingBroker loggingBroker;

        public OpenTableOrchestrationService(IOpenTableService openTableService, ILoggingBroker loggingBroker)
        {
            this.openTableService = openTableService;
            this.loggingBroker = loggingBroker;
        }
        public ValueTask<List<Restaurant>> SearchRestaurantsAsync(RestaurantSearchCriteria restaurantSearchCriteria) =>
            this.openTableService.SearchRestaurantsAsync(restaurantSearchCriteria);

        public async ValueTask<RestaurantReservation> ReserveRestaurantAsync(RestaurantReservationCriteria restaurantReservationCriteria)
        {
            ExternalOpenTableRestaurantsAvailabilityResult externalOpenTableRestaurantsAvailabilityResult = await this.openTableService.SearchRestaurantAvailabilityAsync(restaurantReservationCriteria);

            // TODO: Filter here on preferred day/time and that is availabile.
            ExternalOpenTableRestaurantAvailabilityDay externalOpenTableRestaurantAvailabilityDay = externalOpenTableRestaurantsAvailabilityResult.Data.Availability.First().AvailabilityDays.First();
            ExternalOpenTableRestaurantAvailabilitySlot externalOpenTableRestaurantAvailabilitySlot = externalOpenTableRestaurantAvailabilityDay.slots.First();

            int restaurantId = externalOpenTableRestaurantsAvailabilityResult.Data.Availability.First().RestaurantId;
            int partySize = restaurantReservationCriteria.PartySize;
            string slotHash = externalOpenTableRestaurantAvailabilitySlot.SlotHash;
            string reservationType = externalOpenTableRestaurantAvailabilitySlot.PointsType.ToUpper();

            DateTimeOffset slotDateTime = restaurantReservationCriteria.DateTime
                .AddDays(externalOpenTableRestaurantAvailabilityDay.DayOffset)
                    .AddMinutes(externalOpenTableRestaurantAvailabilitySlot.TimeOffsetMinutes);

            string slotDateTimeString = slotDateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm");


            var externalOpenTableSlotLockBooking = new ExternalOpenTableSlotLockBooking
            {
                Variables = new ExternalOpenTableSlotLockBookingVariables
                {
                    SlotLockInput = new ExternalOpenTableSlotLockBookingSlotLockInput
                    {
                        SlotHash = slotHash,
                        RestaurantId = restaurantId,
                        PartySize = partySize,
                        ReservationDateTime = slotDateTimeString,
                        ReservationType = reservationType,
                        SeatingOption = "DEFAULT"
                    }
                }
            };

            ExternalOpenTableSlotLockBookingResult externalOpenTableSlotLockBookingResult = await this.openTableService.BookRestaurantLockSlotAsync(externalOpenTableSlotLockBooking);

            var externalOpenTableBooking = new ExternalOpenTableBooking
            {
                RestaurantId = restaurantId,
                SlotAvailabilityToken = "eyJ2IjoyLCJtIjoxLCJwIjowLCJzIjowLCJuIjowfQ",
                SlotHash = slotHash,
                SlotLockId = externalOpenTableSlotLockBookingResult.Data.LockedSlot.SlotLock.Id,
                ReservationDateTime = slotDateTimeString,
                PartySize = partySize,
                FirstName = "Z",
                LastName = "Z",
                Email = "andrew.m.mcclelland+OpenTable@gmail.com",
                PhoneNumber = "2064348354",
                PhoneNumberCountryId = "US",
                Country = "US",
                ReservationType = reservationType,
                ReservationAttribute = "default",
                PointsType = "None",
                OptInEmailRestaurant = false
            };

            ExternalOpenTableBookingResult externalOpenTableBookingResult = await this.openTableService.BookRestaurantAsyncAsync(externalOpenTableBooking);

            var restaurantReservation = new RestaurantReservation
            {
                BookingSystem = RestaurantBookingSystem.OpenTable,
                RestaurantId = externalOpenTableBookingResult.RestaurantId.ToString(),
                ReservationId = externalOpenTableBookingResult.ReservationId.ToString(),
                DateTime = DateTimeOffset.Parse(externalOpenTableBookingResult.ReservationDateTime),
                PartySize = externalOpenTableBookingResult.PartySize,
                ConfirmationNumber = externalOpenTableBookingResult.ConfirmationNumber.ToString(),
                ReservationToken = externalOpenTableBookingResult.SecurityToken,
                Environment = externalOpenTableBookingResult.Environment
            };

            return restaurantReservation;
        }
    }
}
