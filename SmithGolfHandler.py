import re
import time
import logging
import requests
from datetime import date, datetime, timedelta
from urllib.parse import quote_plus
from bs4 import BeautifulSoup

from GolfHandler import GolfHandler
from TwilioHandler import TwilioHandler

class SmithGolfHandler(GolfHandler):
    def __init__(self, username: str, password: str, numberHoles: str, numberPlayers: str, preferredTeeTimeRanges: str, playerIdentifier: str, baseUrl: str, loginEndpoint: str, searchTimesEndpoint: str, submitCartEndpoint: str, bookTimeEnabled: bool, twilioHandler: TwilioHandler, logger):
        super().__init__(numberHoles=numberHoles, numberPlayers=numberPlayers, preferredTeeTimeRanges=preferredTeeTimeRanges, baseUrl=baseUrl, bookTimeEnabled=bookTimeEnabled, twilioHandler=twilioHandler, logger=logger)

        self.username = username
        self.password = password
        self.playerIdentifier = playerIdentifier
        self.loginUrl = baseUrl + loginEndpoint
        self.searchTimesUrl = baseUrl + searchTimesEndpoint
        self.submitCartUrl = baseUrl + submitCartEndpoint

    def BookTeeTimes(self):
        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_Start")

        # Book for date 7 days from today
        today = datetime.now()
        dayInAWeek = today + timedelta(days=7)
        dayInAWeekString = dayInAWeek.strftime('%m/%d/%Y')

        # Login to site
        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_Login")
        
        loginPayload = {
            'Action': 'process',
            'SubAction': '',
            'weblogin_username': self.username,
            'weblogin_password': self.password,
            'weblogin_buttonlogin': 'Login'
        }
        loginResponse = self.session.post(url=self.loginUrl, data=loginPayload)

        # Search for teetimes and store in dictionary
        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_SearchTeeTimes")

        searchDate = quote_plus(dayInAWeekString)
        searchTime = quote_plus("06:00 AM")

        searchTimesResponse = self.session.get(url=self.searchTimesUrl.format(self.numberPlayers, searchDate, searchTime, self.numberHoles))

        parsedSearchTimes = BeautifulSoup(searchTimesResponse.text)
        teeTimeRows = parsedSearchTimes.find_all('table', attrs={'id': 'grwebsearch_output_table'})[0].find_all('tr')

        availableTeeTimesDict = {}

        for teeTimeRow in teeTimeRows[1:]:
            teeTimeAddToCartUrl = teeTimeRow.contents[1].contents[0].attrs['href']
            teeTimeIsAvailable = teeTimeRow.contents[1].text == "Add To Cart"
            teeTime = teeTimeRow.contents[3].contents[0].strip()
            teeTimeDate = teeTimeRow.contents[5].contents[0]
            teeTimeHoles = teeTimeRow.contents[7].contents[0]
            teeTimeCourse = teeTimeRow.contents[9].contents[0]
            teeTimeOpenSlots = teeTimeRow.contents[11].contents[0]

            availableTeeTimesDict[teeTime] = {
                'AddToCartUrl': teeTimeAddToCartUrl,
                'IsAddToCartAvailable': teeTimeIsAvailable,
                'Time' : teeTime,
                'Date': teeTimeDate,
                'Holes': teeTimeHoles,
                'Course': teeTimeCourse,
                'OpenSlots': teeTimeOpenSlots
            }

        sortedAvailableTeeTimes = super()._SortAvailableTeeTimes(list(availableTeeTimesDict.keys()))

        # Select available tee time that has highest preference
        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_FindAvailablePreferredTeeTime")

        # Booking available tee time in order of preferred times
        for teeTime in sortedAvailableTeeTimes:
            if (availableTeeTimesDict[teeTime]['IsAddToCartAvailable'] == True) and (availableTeeTimesDict[teeTime]['Holes'] == '18 (Front)') and (availableTeeTimesDict[teeTime]['Course'] == 'H. Smith Richardson Golf Course') and (availableTeeTimesDict[teeTime]['OpenSlots'] == '4'):

                self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeAttempt", extra={'custom_dimensions': {'Date': availableTeeTimesDict[teeTime]['Date'], 'TeeTime': teeTime}})

                if self.bookTimeEnabled:
                    # Add tee time to the cart
                    self.logger.info("SmithGolfHandler.BookSmithTeeTimes_AddTeeTimeToCart", extra={'custom_dimensions': {'Date': availableTeeTimesDict[teeTime]['Date'], 'TeeTime': teeTime}})

                    addToCartResponse = self.session.get(availableTeeTimesDict[teeTime]['AddToCartUrl'])

                    # Submit cart with tee time
                    self.logger.info("SmithGolfHandler.BookSmithTeeTimes_SubmitTeeTimeCart", extra={'custom_dimensions': {'Date': availableTeeTimesDict[teeTime]['Date'], 'TeeTime': teeTime}})

                    submitCartPayload = {
                        'Action': 'process',
                        'SubAction': '',
                        'golfmemberselection_player1': self.playerIdentifier,
                        'golfmemberselection_player2': self.playerIdentifier,
                        'golfmemberselection_player3': self.playerIdentifier,
                        'golfmemberselection_player4': self.playerIdentifier,
                        'golfmemberselection_player5': 'Skip',
                        'golfmemberselection_buttononeclicktofinish': 'One Click To Finish'
                    }
                    submitCartResponse = self.session.post(self.submitCartUrl, data=submitCartPayload)

                    # Confirm success of booking
                    self.logger.info("SmithGolfHandler.BookSmithTeeTimes_ConfirmSuccess", extra={'custom_dimensions': {'Date': availableTeeTimesDict[teeTime]['Date'], 'TeeTime': teeTime}})
                    try:
                        parsedSubmitCart = BeautifulSoup(submitCartResponse.text)
                        bookingConfirmed = parsedSubmitCart.body.find('h1', attrs={'id':'webconfirmation_pageheader'}).text == "Your Online transaction is complete. Please select an option below to continue."
                    except:
                        bookingConfirmed = False

                    # Successfully booked Tee time
                    if bookingConfirmed:
                        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeSuccess", extra={'custom_dimensions': {'Date': availableTeeTimesDict[teeTime]['Date'], 'TeeTime': teeTime}})
                        successMessage = "{} {} - booked {} teetime on {} at {} for {} people.".format(today.strftime('%m/%d/%Y'), datetime.now().strftime("%H:%M:%S"), teeTime, availableTeeTimesDict[teeTime]['Date'], availableTeeTimesDict[teeTime]['Course'], availableTeeTimesDict[teeTime]['OpenSlots'])
                        self.twilioHandler.sendSms(successMessage, not self.bookTimeEnabled)
                        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_TwilioSuccessSent")
                        break
                    # Failed to book Tee Time, retrying with next preferred time
                    else:
                        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeFail", extra={'custom_dimensions': {'Date': availableTeeTimesDict[teeTime]['Date'], 'TeeTime': teeTime}})
                        failMessage = "{} {} - FAILED to book {} teetime on {} at {} for {} people. Trying next preferred teetime...".format(today.strftime('%m/%d/%Y'), datetime.now().strftime("%H:%M:%S"), teeTime, availableTeeTimesDict[teeTime]['Date'], availableTeeTimesDict[teeTime]['Course'], availableTeeTimesDict[teeTime]['OpenSlots'])
                        self.twilioHandler.sendSms(failMessage, True)
                        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_TwilioFailSent")

                if not self.bookTimeEnabled:
                    break

        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_End")