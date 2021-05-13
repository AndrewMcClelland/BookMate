import re
import time
import logging
from datetime import date, datetime, timedelta
from urllib.parse import quote_plus
from bs4 import BeautifulSoup

import requests

from TwilioHandler import TwilioHandler

class SmithGolfHandler:
    def __init__(self, preferredTeeTimes, username: str, password: str, playerIdentifier: str, baseUrl: str, loginEndpoint: str, searchTimesEndpoint: str, submitCartEndpoint: str, isDevMode: bool, twilioHandler: TwilioHandler, logger):
        self.username = username
        self.password = password
        self.playerIdentifier = playerIdentifier
        self.preferredTeeTimes = preferredTeeTimes
        self.loginUrl = baseUrl + loginEndpoint
        self.searchTimesUrl = baseUrl + searchTimesEndpoint
        self.submitCartUrl = baseUrl + submitCartEndpoint
        self.isDevMode = isDevMode
        self.twilioHandler = twilioHandler
        self.logger = logger

        self.session = requests.Session()

    def BookSmithTeeTimes(self):
        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_Start")

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

        # Search for teetimes
        numberPlayers = "4"
        numberHoles = "18"
        searchDate = quote_plus(dayInAWeekString)
        searchTime = quote_plus("06:00 AM")

        searchTimesResponse = self.session.get(url=self.searchTimesUrl.format(numberPlayers, searchDate, searchTime, numberHoles))

        parsedSearchTimes = BeautifulSoup(searchTimesResponse.text)
        teeTimeRows = parsedSearchTimes.find_all('table', attrs={'id': 'grwebsearch_output_table'})[0].find_all('tr')

        teeTimesDict = {}

        for teeTimeRow in teeTimeRows[1:]:
            teeTimeAddToCartUrl = teeTimeRow.contents[1].contents[0].attrs['href']
            teeTimeIsAvailable = teeTimeRow.contents[1].text == "Add To Cart"
            teeTime = teeTimeRow.contents[3].contents[0].strip()
            teeTimeDate = teeTimeRow.contents[5].contents[0]
            teeTimeHoles = teeTimeRow.contents[7].contents[0]
            teeTimeCourse = teeTimeRow.contents[9].contents[0]
            teeTimeOpenSlots = teeTimeRow.contents[11].contents[0]

            teeTimesDict[teeTime] = {
                'AddToCartUrl': teeTimeAddToCartUrl,
                'IsAddToCartAvailable': teeTimeIsAvailable,
                'Time' : teeTime,
                'Date': teeTimeDate,
                'Holes': teeTimeHoles,
                'Course': teeTimeCourse,
                'OpenSlots': teeTimeOpenSlots
            }

        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_AvailableTimes")

        # Booking available tee time in order of preferred times
        for teeTime in self.preferredTeeTimes:
            if (teeTime in teeTimesDict) and (teeTimesDict[teeTime]['IsAddToCartAvailable'] == True) and (teeTimesDict[teeTime]['Holes'] == '18 (Front)') and (teeTimesDict[teeTime]['Course'] == 'H. Smith Richardson Golf Course') and (teeTimesDict[teeTime]['OpenSlots'] == '4'):

                self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeAttempt", extra={'custom_dimensions': {'TeeTime': teeTime}})

                if not self.isDevMode:
                    self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeAttempt", extra={'custom_dimensions': {'TeeTime': teeTime}})

                    addToCartResponse = self.session.get(teeTimesDict[teeTime]['AddToCartUrl'])

                    self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeAttempt", extra={'custom_dimensions': {'TeeTime': teeTime}})

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

                    try:
                        parsedSubmitCart = BeautifulSoup(submitCartResponse.text)
                        bookingConfirmed = parsedSubmitCart.body.find('h1', attrs={'id':'webconfirmation_pageheader'}).text == "Your Online transaction is complete. Please select an option below to continue."
                    except:
                        bookingConfirmed = False

                    if bookingConfirmed:
                        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeSuccess", extra={'custom_dimensions': {'TeeTime': teeTime}})
                        successMessage = "{} {} - booked {} teetime on {} at {} for {} people.".format(today.strftime('%m/%d/%Y'), datetime.now().strftime("%H:%M:%S"), teeTime, teeTimesDict[teeTime]['Date'], teeTimesDict[teeTime]['Course'], teeTimesDict[teeTime]['OpenSlots'])
                        self.twilioHandler.sendSms(successMessage, self.isDevMode)
                        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_TwilioSuccessSent")
                        break
                    # Failed to book Tee Time, retrying with next preferred time
                    else:
                        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeFail", extra={'custom_dimensions': {'TeeTime': teeTime}})
                        failMessage = "{} {} - FAILED to book {} teetime on {} at {} for {} people. Trying next preferred teetime...".format(today.strftime('%m/%d/%Y'), datetime.now().strftime("%H:%M:%S"), teeTime, teeTimesDict[teeTime]['Date'], teeTimesDict[teeTime]['Course'], teeTimesDict[teeTime]['OpenSlots'])
                        self.twilioHandler.sendSms(failMessage, True)
                        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_TwilioFailSent")

                if self.isDevMode:
                    break

        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_End")