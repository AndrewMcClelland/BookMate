import re
import time
import logging
from datetime import date, datetime, timedelta

import requests
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support.ui import Select

from handlers.twilio_handler import TwilioHandler

class SmithGolfHandlerSelenium:
    def __init__(self, url: str, preferred_tee_times, username: str, password: str, is_dev_mode: bool, twilio_handler: TwilioHandler, logger):
        self.url = url
        self.username = username
        self.password = password
        self.preferred_tee_times = preferred_tee_times.split(',')
        self.is_dev_mode = is_dev_mode
        self.twilio_handler = twilio_handler
        self.logger = logger

        options = Options()
        options.headless = True
        self.driver = webdriver.Chrome(options=options)
        self.driver.implicitly_wait(30)
        self.driver.maximize_window()

    def book_smith_tee_times(self):
        
        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_Start")

        self.driver.get(self.url)

        assert "Fairfield Parks and Recreation Search" in self.driver.title

        today = datetime.now()
        day_in_a_week = today + timedelta(days=7)
        day_in_a_week_string = day_in_a_week.strftime('%m/%d/%Y')
        is_next_month = today.month != day_in_a_week.month

        #self.driver.get_screenshot_as_file('screenshots/smithgolf_mainpage_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))

        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_Login")

        # Login
        self.driver.find_element_by_xpath("//a[@class='login-link popup-link']").click()
        #self.driver.get_screenshot_as_file('screenshots/smithgolf_LoginEmpty_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))
        self.driver.find_element_by_id("weblogin_username").clear()
        self.driver.find_element_by_id("weblogin_username").send_keys(self.username)
        self.driver.find_element_by_id("weblogin_password").clear()
        self.driver.find_element_by_id("weblogin_password").send_keys(self.password)
        #self.driver.get_screenshot_as_file('screenshots/smithgolf_LoginFilled_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))
        self.driver.find_element_by_xpath("//input[@id='weblogin_buttonlogin']").click()

        # Homepage - Navigate to Golf
        #self.driver.get_screenshot_as_file('screenshots/smithgolf_HomePageLoggedIn_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))
        self.driver.get(self.url)
        #self.driver.get_screenshot_as_file('screenshots/smithgolf_LoggedInGolfTeeTimes_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))

        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_TeeTimeSearch")

        # TeeTime settings
        course_dropdown_xpath = "//select[@name='secondarycode']/option[text()='H. Smith Richardson Golf Course']"
        num_players_dropdown_xpath = "//select[@name='numberofplayers']/option[text()='4']"
        num_holes_dropdown_xpath = "//select[@name='numberofholes']/option[text()='18 holes']"
        begin_time_dropdown_xpath = "//input[@id='begintime']"
        begin_time_input_xpath = "//div[@id='begintime_root']/div/div/div/div/ul/li[text()='06:00 AM']"
        begin_date_dropdown_xpath = "//input[@name='begindate']"
        next_month_xpath = "//div[@title='Next month']"
        begin_date_input_xpath = "//div[@id='begindate_root']/div/div/div/div/table/tbody//td/div[@aria-label='{}']".format(day_in_a_week_string)
        search_tee_times_button_xpath = "//input[@name='grwebsearch_buttonsearch']"
        search_tee_times_table = "//table[@id='grwebsearch_output_table']"

        self.driver.find_element_by_xpath(course_dropdown_xpath).click()
        self.driver.find_element_by_xpath(num_players_dropdown_xpath).click()
        self.driver.find_element_by_xpath(num_holes_dropdown_xpath).click()
        self.driver.find_element_by_xpath(begin_time_dropdown_xpath).click()
        self.driver.find_element_by_xpath(begin_time_input_xpath).click()
        self.driver.find_element_by_xpath(begin_date_dropdown_xpath).click()
        if(is_next_month):
            self.driver.find_element_by_xpath(next_month_xpath).click()
        self.driver.find_element_by_xpath(begin_date_input_xpath).click()
        self.driver.find_element_by_xpath(search_tee_times_button_xpath).click()

        #self.driver.get_screenshot_as_file('screenshots/smithgolf_teetimes_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))

        # Getting all available TeeTimes
        tee_time_rows = self.driver.find_element_by_xpath(search_tee_times_table).find_elements_by_css_selector('tr')
        tee_time_cells = self.driver.find_element_by_xpath(search_tee_times_table).find_elements_by_css_selector('td')

        tee_times_grouped = [tee_time_cells[n: n + 8] for n in range(0, len(tee_time_cells), 8)]
        tee_times_dict = {tee_times_grouped[i][1].text : {'AddToCartIndex': str(i), 'IsAddToCartAvailable': str(tee_times_grouped[i][0].text == 'Add To Cart'), 'Date' : tee_times_grouped[i][2].text, 'Holes': tee_times_grouped[i][3].text, 'Course': tee_times_grouped[i][4].text, 'OpenSlots': tee_times_grouped[i][5].text } for i in range(len(tee_times_grouped))}

        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_AvailableTimes")

        # Booking available tee time in order of preferred times
        for tee_time in self.preferred_tee_times:
            if (tee_time in tee_times_dict) and (tee_times_dict[tee_time]['IsAddToCartAvailable'] == 'True') and (tee_times_dict[tee_time]['Holes'] == '18 (Front)') and (tee_times_dict[tee_time]['Course'] == 'H. Smith Richardson Golf Course') and (tee_times_dict[tee_time]['OpenSlots'] == '4'):

                self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeAttempt", extra={'custom_dimensions': {'TeeTime': tee_time}})

                add_to_cart_elements = self.driver.find_elements_by_xpath("//a[@title='Add To Cart' or @title='Unavailable']")

                if not self.is_dev_mode:
                    add_to_cart_elements[int(tee_times_dict[tee_time]['AddToCartIndex'])].click()

                    #self.driver.get_screenshot_as_file('screenshots/smithgolf_personalinfo_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))
                    self.driver.find_element_by_id('golfmemberselection_buttononeclicktofinish').click()

                # Successfully booked Tee Time
                try:
                    time.sleep(5)
                    #self.driver.get_screenshot_as_file('screenshots/smithgolf_submitted_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))
                    self.driver.find_element_by_id('webconfirmation_pageheader')
                    self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeSuccess", extra={'custom_dimensions': {'TeeTime': tee_time}})
                    success_message = "{} {} - booked {} teetime on {} at {} for {} people.".format(today.strftime('%m/%d/%Y'), datetime.now().strftime("%H:%M:%S"), tee_time, tee_times_dict[tee_time]['Date'], tee_times_dict[tee_time]['Course'], tee_times_dict[tee_time]['OpenSlots'])
                    self.twilio_handler.send_sms(success_message, self.is_dev_mode)
                    self.logger.info("SmithGolfHandler.BookSmithTeeTimes_TwilioSuccessSent")
                    break
                # Failed to book Tee Time, retrying with next preferred time
                except:
                    #self.driver.get_screenshot_as_file('screenshots/smithgolf_failed_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))
                    self.logger.info("SmithGolfHandler.BookSmithTeeTimes_BookTimeFail", extra={'custom_dimensions': {'TeeTime': tee_time}})
                    fail_message = "{} {} - FAILED to book {} teetime on {} at {} for {} people. Trying next preferred teetime...".format(today.strftime('%m/%d/%Y'), datetime.now().strftime("%H:%M:%S"), tee_time, tee_times_dict[tee_time]['Date'], tee_times_dict[tee_time]['Course'], tee_times_dict[tee_time]['OpenSlots'])
                    self.twilio_handler.send_sms(fail_message, True)
                    self.logger.info("SmithGolfHandler.BookSmithTeeTimes_TwilioFailSent")
                    self.driver.back()
                    time.sleep(1)

                if self.is_dev_mode:
                    break

        self.logger.info("SmithGolfHandler.BookSmithTeeTimes_DriverQuit")
        self.driver.quit()