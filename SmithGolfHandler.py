import re
import time
from datetime import date, datetime, timedelta

import requests
from selenium import webdriver
from selenium.webself.driver.chrome.options import Options
from selenium.webself.driver.common.keys import Keys
from selenium.webself.driver.support.ui import Select
from TwilioHandler import TwilioHandler

class SmithGolfHandler:
    def __init__(self, url: str, preferredTeeTimes, username: str, password: str, twilioHandler: TwilioHandler):
        self.url = url
        self.username = username
        self.password = password
        self.preferredTeeTimes = preferredTeeTimes
        self.twilioHandler = twilioHandler

        options = Options()
        options.headless = True
        self.driver = webdriver.Chrome(options=options)
        self.driver.implicitly_wait(30)
        self.driver.maximize_window()

    def BookSmithTeeTimes(self):
        
        self.self.driver.get(self.url)

        assert "Fairfield Parks and Recreation Search" in driver.title

        today = datetime.now()
        dayInAWeek = today + timedelta(days=7)
        dayInAWeekString = dayInAWeek.strftime('%m/%d/%Y')
        isNextMonth = today.month != dayInAWeek.month

        #driver.get_screenshot_as_file('screenshots/smithgolf_mainpage_{}-{}.png'.format(today.strftime('%m.%d.%Y'), timeToRun.replace(':', '-')))

        # Login
        driver.find_element_by_xpath("//a[@class='login-link popup-link']").click()
        #driver.get_screenshot_as_file('screenshots/smithgolf_LoginEmpty_{}-{}.png'.format(today.strftime('%m.%d.%Y'), timeToRun.replace(':', '-')))
        driver.find_element_by_id("weblogin_username").clear()
        driver.find_element_by_id("weblogin_username").send_keys(self.username)
        driver.find_element_by_id("weblogin_password").clear()
        driver.find_element_by_id("weblogin_password").send_keys(self.username)
        #driver.get_screenshot_as_file('screenshots/smithgolf_LoginFilled_{}-{}.png'.format(today.strftime('%m.%d.%Y'), timeToRun.replace(':', '-')))
        driver.find_element_by_xpath("//input[@id='weblogin_buttonlogin']").click()

        # Homepage - Navigate to Golf
        #driver.get_screenshot_as_file('screenshots/smithgolf_HomePageLoggedIn_{}-{}.png'.format(today.strftime('%m.%d.%Y'), timeToRun.replace(':', '-')))
        driver.get(self.url)
        #driver.get_screenshot_as_file('screenshots/smithgolf_LoggedInGolfTeeTimes_{}-{}.png'.format(today.strftime('%m.%d.%Y'), timeToRun.replace(':', '-')))

        # TeeTime settings
        courseDropdownXPath = "//select[@name='secondarycode']/option[text()='H. Smith Richardson Golf Course']"
        numPlayersDropdownXpath = "//select[@name='numberofplayers']/option[text()='4']"
        numHolesDropdownXpath = "//select[@name='numberofholes']/option[text()='18 holes']"
        beginTimeDropdownXPath = "//input[@id='begintime']"
        beginTimeInputXPath = "//div[@id='begintime_root']/div/div/div/div/ul/li[text()='06:00 AM']"
        beginDateDropdownXPath = "//input[@name='begindate']"
        nextMonthXPath = "//div[@title='Next month']"
        beginDateInputXPath = "//div[@id='begindate_root']/div/div/div/div/table/tbody//td/div[@aria-label='{}']".format(dayInAWeekString)
        searchTeeTimesButton = "//input[@name='grwebsearch_buttonsearch']"
        searchTeeTimesTable = "//table[@id='grwebsearch_output_table']"

        driver.find_element_by_xpath(courseDropdownXPath).click()
        driver.find_element_by_xpath(numPlayersDropdownXpath).click()
        driver.find_element_by_xpath(numHolesDropdownXpath).click()
        driver.find_element_by_xpath(beginTimeDropdownXPath).click()
        driver.find_element_by_xpath(beginTimeInputXPath).click()
        driver.find_element_by_xpath(beginDateDropdownXPath).click()
        if(isNextMonth):
            driver.find_element_by_xpath(nextMonthXPath).click()
        driver.find_element_by_xpath(beginDateInputXPath).click()
        driver.find_element_by_xpath(searchTeeTimesButton).click()

        #driver.get_screenshot_as_file('screenshots/smithgolf_teetimes_{}-{}.png'.format(today.strftime('%m.%d.%Y'), timeToRun.replace(':', '-')))

        # Getting all available TeeTimes
        teeTimeRows = driver.find_element_by_xpath(searchTeeTimesTable).find_elements_by_css_selector('tr')
        teeTimeCells = driver.find_element_by_xpath(searchTeeTimesTable).find_elements_by_css_selector('td')

        teeTimesGrouped = [teeTimeCells[n: n + 8] for n in range(0, len(teeTimeCells), 8)]
        teeTimesDict = {teeTimesGrouped[i][1].text : {'AddToCartIndex': str(i), 'IsAddToCartAvailable': str(teeTimesGrouped[i][0].text == 'Add To Cart'), 'Date' : teeTimesGrouped[i][2].text, 'Holes': teeTimesGrouped[i][3].text, 'Course': teeTimesGrouped[i][4].text, 'OpenSlots': teeTimesGrouped[i][5].text } for i in range(len(teeTimesGrouped))}
        #teeTimeRowHeaders = ['AddToCart', 'Time', 'Date', 'Holes', 'Course', 'Open', 'Slots', 'Status']

        # preferredTeeTimesInOrder = ['10:30 am', '10:39 am', '10:48 am', '10:57 am', '11:06 am', '11:15 am', '11:24 am', '11:33 am', '11:42 am', '11:51 am', '12:00 pm', '12:09 pm', '12:18 pm', '12:27 pm', '12:36 pm', '12:45 pm', '12:54 pm', '1:03 pm', '1:12 pm', '1:21 pm', '1:30 pm', '1:39 pm', '1:48 pm', '1:57 pm', '10:21 am', '10:12 am', '10:03 am', '9:54 am', '9:45 am', '9:36 am', '9:27 am', '9:18 am', '9:09 am', '9:00 am', '2:06 pm', '2:15 pm', '2:24 pm', '2:33 pm', '2:42 pm', '8:51 am', '8:42 am', '8:33 am', '8:24 am', '2:51 pm', '3:00 pm', '3:09 pm', '3:18 pm', '3:27 pm', '3:36 pm', '3:45 pm', '3:54 pm', '4:03 pm', '4:12 pm', '4:21 pm', '4:30 pm', '4:39 pm', '4:48 pm', '4:57 pm', '5:06 pm', '5:15 pm', '5:24 pm', '5:33 pm', '5:42 pm', '5:51 pm', '6:00 pm', '6:09 pm', '6:18 pm', '6:27 pm', '6:36 pm', '6:45 pm', '6:54 pm']
        preferredTeeTimesInOrder = ['1:30 pm', '1:39 pm', '1:48 pm', '1:57 pm', '2:06 pm', '2:15 pm', '2:24 pm', '2:33 pm', '2:42 pm', '2:51 pm', '3:00 pm', '3:09 pm', '3:18 pm', '3:27 pm', '1:12 pm', '1:21 pm', '3:36 pm', '3:45 pm', '3:54 pm', '4:03 pm', '10:30 am', '10:39 am', '10:48 am', '10:57 am', '11:06 am', '11:15 am', '11:24 am', '11:33 am', '11:42 am', '11:51 am', '12:00 pm', '12:09 pm', '12:18 pm', '12:27 pm', '12:36 pm', '12:45 pm', '12:54 pm', '1:03 pm', '10:21 am', '10:12 am', '10:03 am', '9:54 am', '9:45 am', '9:36 am', '9:27 am', '9:18 am', '9:09 am', '9:00 am', '8:51 am', '8:42 am', '8:33 am', '8:24 am', '2:51 pm', '3:00 pm', '3:09 pm', '3:18 pm', '3:27 pm', '3:36 pm', '3:45 pm', '3:54 pm', '4:03 pm', '4:12 pm', '4:21 pm', '4:30 pm', '4:39 pm', '4:48 pm', '4:57 pm', '5:06 pm', '5:15 pm', '5:24 pm', '5:33 pm', '5:42 pm', '5:51 pm', '6:00 pm', '6:09 pm', '6:18 pm', '6:27 pm', '6:36 pm', '6:45 pm', '6:54 pm']

        # Booking available tee time in order of preferred times
        for teeTime in preferredTeeTimesInOrder:
            if (teeTime in teeTimesDict) and (teeTimesDict[teeTime]['IsAddToCartAvailable'] == 'True') and (teeTimesDict[teeTime]['Holes'] == '18 (Front)') and (teeTimesDict[teeTime]['Course'] == 'H. Smith Richardson Golf Course') and (teeTimesDict[teeTime]['OpenSlots'] == '4'):
                addToCartElements = driver.find_elements_by_xpath("//a[@title='Add To Cart' or @title='Unavailable']")
                addToCartElements[int(teeTimesDict[teeTime]['AddToCartIndex'])].click()

                #driver.get_screenshot_as_file('screenshots/smithgolf_personalinfo_{}-{}.png'.format(today.strftime('%m.%d.%Y'), timeToRun.replace(':', '-')))
                driver.find_element_by_id('golfmemberselection_buttononeclicktofinish').click()

                # Successfully booked Tee Time
                try:
                    time.sleep(5)
                    #driver.get_screenshot_as_file('screenshots/smithgolf_submitted_{}-{}.png'.format(today.strftime('%m.%d.%Y'), timeToRun.replace(':', '-')))
                    driver.find_element_by_id('webconfirmation_pageheader')
                    successMessage = "{} {} - booked {} teetime on {} at {} for {} people.".format(today.strftime('%m/%d/%Y'), datetime.now().strftime("%H:%M:%S"), teeTime, teeTimesDict[teeTime]['Date'], teeTimesDict[teeTime]['Course'], teeTimesDict[teeTime]['OpenSlots'])
                    print(successMessage)
                    self.twilioHandler.sendSms(successMessage)
                    break
                # Failed to book Tee Time, retrying with next preferred time
                except:
                    #driver.get_screenshot_as_file('screenshots/smithgolf_failed_{}-{}.png'.format(today.strftime('%m.%d.%Y'), timeToRun.replace(':', '-')))
                    failMessage = "{} {} - FAILED to book {} teetime on {} at {} for {} people. Trying next preferred teetime...".format(today.strftime('%m/%d/%Y'), datetime.now().strftime("%H:%M:%S"), teeTime, teeTimesDict[teeTime]['Date'], teeTimesDict[teeTime]['Course'], teeTimesDict[teeTime]['OpenSlots'])
                    print(failMessage)
                    self.twilioHandler.sendSms(failMessage, True)
                    driver.back()
                    time.sleep(1)
        
        print("\nQuitting...")
        driver.quit()