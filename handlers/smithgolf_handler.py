from datetime import datetime
from urllib.parse import quote_plus
from bs4 import BeautifulSoup

from handlers.golf_handler import GolfHandler
from handlers.twilio_handler import TwilioHandler

class SmithGolfHandler(GolfHandler):
    def __init__(self, username: str, password: str, number_holes: str, number_players: str, preferred_tee_time_ranges: str, days_to_book_in_advance: str, player_identifier: str, base_url: str, login_endpoint: str, search_times_endpoint: str, submit_cart_endpoint: str, book_time_enabled: bool, twilio_handler: TwilioHandler, logger):
        super().__init__(number_holes=number_holes, number_players=number_players, preferred_tee_time_ranges=preferred_tee_time_ranges, days_to_book_in_advance=days_to_book_in_advance, base_url=base_url, book_time_enabled=book_time_enabled, twilio_handler=twilio_handler, logger=logger)

        self.username = username
        self.password = password
        self.player_identifier = player_identifier
        self.login_url = base_url + login_endpoint
        self.search_times_url = base_url + search_times_endpoint
        self.submit_cart_url = base_url + submit_cart_endpoint

    def book_tee_times(self):
        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_Start")

        # Login to site
        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_Login")

        login_payload = {
            'Action': 'process',
            'SubAction': '',
            'weblogin_username': self.username,
            'weblogin_password': self.password,
            'weblogin_buttonlogin': 'Login'
        }
        login_response = self.session.post(url=self.login_url, data=login_payload)

        # Search for teetimes and store in dictionary
        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_SearchTeeTimes")

        search_date = quote_plus(self.date_to_book.strftime('%m/%d/%Y'))
        search_time = quote_plus("06:00 AM")

        search_times_response = self.session.get(url=self.search_times_url.format(self.number_players, search_date, search_time, self.number_holes))

        parsed_search_times = BeautifulSoup(search_times_response.text)
        tee_time_rows = parsed_search_times.find_all('table', attrs={'id': 'grwebsearch_output_table'})[0].find_all('tr')

        available_tee_times_dict = {}

        for tee_time_row in tee_time_rows[1:]:
            tee_time_add_to_cart_url = tee_time_row.contents[1].contents[0].attrs['href']
            tee_time_is_available = tee_time_row.contents[1].text == "Add To Cart"
            tee_time = tee_time_row.contents[3].contents[0].strip()
            tee_time_date = tee_time_row.contents[5].contents[0]
            tee_time_holes = tee_time_row.contents[7].contents[0]
            tee_time_course = tee_time_row.contents[9].contents[0]
            tee_time_open_slots = tee_time_row.contents[11].contents[0]

            available_tee_times_dict[tee_time] = {
                'AddToCartUrl': tee_time_add_to_cart_url,
                'IsAddToCartAvailable': tee_time_is_available,
                'Time' : tee_time,
                'Date': tee_time_date,
                'Holes': tee_time_holes,
                'Course': tee_time_course,
                'OpenSlots': tee_time_open_slots
            }

        sorted_available_tee_times = super()._sort_available_tee_times(list(available_tee_times_dict.keys()))

        # Select available tee time that has highest preference
        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_FindAvailablePreferredTeeTime")

        # Booking available tee time in order of preferred times
        for tee_time in sorted_available_tee_times:
            if (available_tee_times_dict[tee_time]['IsAddToCartAvailable'] is True) and (available_tee_times_dict[tee_time]['Holes'] == '18 (Front)') and (available_tee_times_dict[tee_time]['Course'] == 'H. Smith Richardson Golf Course') and (available_tee_times_dict[tee_time]['OpenSlots'] == '4'):

                self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeAttempt", extra={'custom_dimensions': {'Date': available_tee_times_dict[tee_time]['Date'], 'TeeTime': tee_time}})

                if self.book_time_enabled:
                    # Add tee time to the cart
                    self.logger.info("SmithGolfHandler.BookSmithTeeTimes_AddTeeTimeToCart", extra={'custom_dimensions': {'Date': available_tee_times_dict[tee_time]['Date'], 'TeeTime': tee_time}})

                    add_to_cart_response = self.session.get(available_tee_times_dict[tee_time]['AddToCartUrl'])

                    # Submit cart with tee time
                    self.logger.info("SmithGolfHandler.BookSmithTeeTimes_SubmitTeeTimeCart", extra={'custom_dimensions': {'Date': available_tee_times_dict[tee_time]['Date'], 'TeeTime': tee_time}})

                    submit_cart_payload = {
                        'Action': 'process',
                        'SubAction': '',
                        'golfmemberselection_player1': self.player_identifier,
                        'golfmemberselection_player2': self.player_identifier,
                        'golfmemberselection_player3': self.player_identifier,
                        'golfmemberselection_player4': self.player_identifier,
                        'golfmemberselection_player5': 'Skip',
                        'golfmemberselection_buttononeclicktofinish': 'One Click To Finish'
                    }
                    submit_cart_response = self.session.post(self.submit_cart_url, data=submit_cart_payload)

                    # Confirm success of booking
                    self.logger.info("SmithGolfHandler.BookSmithTeeTimes_ConfirmSuccess", extra={'custom_dimensions': {'Date': available_tee_times_dict[tee_time]['Date'], 'TeeTime': tee_time}})
                    try:
                        parsed_submit_cart = BeautifulSoup(submit_cart_response.text)
                        booking_confirmed = parsed_submit_cart.body.find('h1', attrs={'id':'webconfirmation_pageheader'}).text == "Your Online transaction is complete. Please select an option below to continue."
                    except:
                        booking_confirmed = False

                    today = datetime.now()
                    # Successfully booked Tee time
                    if booking_confirmed:
                        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeSuccess", extra={'custom_dimensions': {'Date': available_tee_times_dict[tee_time]['Date'], 'TeeTime': tee_time}})
                        success_message = "{} {} - booked {} teetime on {} at {} for {} people.".format(today.strftime('%m/%d/%Y'), datetime.now().strftime("%H:%M:%S"), tee_time, available_tee_times_dict[tee_time]['Date'], available_tee_times_dict[tee_time]['Course'], available_tee_times_dict[tee_time]['OpenSlots'])
                        self.twilio_handler.send_sms(success_message, not self.book_time_enabled)
                        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_TwilioSuccessSent")
                        break
                    # Failed to book Tee Time, retrying with next preferred time
                    else:
                        # Try to parse Smith Processing error information
                        try:
                            parsed_submit_cart = BeautifulSoup(submit_cart_response.text)
                            parsed_smith_error = parsed_submit_cart.find_all('div', attrs={'id':'processingprompts_ruletext'})
                            smith_error_text = '||'.join([str(error_text.text) for error_text in parsed_smith_error])
                        except:
                            smith_error_text = "Couldn't retrieve Smith Error Prompts."

                        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeFail", extra={'custom_dimensions': {'Date': available_tee_times_dict[tee_time]['Date'], 'TeeTime': tee_time, 'SmithErrorText': smith_error_text}})
                        fail_message = "{} {} - FAILED to book {} teetime on {} at {} for {} people. Trying next preferred teetime...".format(today.strftime('%m/%d/%Y'), datetime.now().strftime("%H:%M:%S"), tee_time, available_tee_times_dict[tee_time]['Date'], available_tee_times_dict[tee_time]['Course'], available_tee_times_dict[tee_time]['OpenSlots'])
                        self.twilio_handler.send_sms(fail_message, True)
                        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_TwilioFailSent")

                if not self.book_time_enabled:
                    break

        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_End")
