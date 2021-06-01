import configparser
import re
import time
from datetime import date, datetime, timedelta

import requests
import schedule
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support.ui import Select
from twilio.rest import Client

config = configparser.ConfigParser()
config.read('./legacy/config.ini')
time_to_run = "05:00:30"

def send_sms(body, send_to_me_only = False):
    account_sid = config['Twilio']['accountSid']
    auth_token = config['Twilio']['auth_token']
    to_numbers = [config['Twilio']['to_numbers'].split(',')[0]] if send_to_me_only else config['Twilio']['to_numbers'].split(',')
    
    client = Client(account_sid, auth_token) 
    
    max_body_chars = 1500
    message_bodies = [body[i: i + max_body_chars] for i in range(0, len(body), max_body_chars)]

    for to_number in to_numbers:
        for message_body in message_bodies:
            message = client.messages.create( 
                                    from_=config['Twilio']['from_number'],
                                    body=message_body,      
                                    to=to_number
                                    )
            
            print("Sent SMS at {} - To: {}\tSID: {}".format(datetime.now().strftime("%H:%M:%S on %m/%d/%Y"), str(message.to), message.sid))

def get_smith_tee_times():
    options = Options()
    # options.headless = True
    driver = webdriver.Chrome(options=options)
    driver.implicitly_wait(30)
    driver.maximize_window()
    driver.get(config['SmithSite']['golfUrl'])

    assert "Fairfield Parks and Recreation Search" in driver.title

    today = datetime.now()
    day_in_a_week = today + timedelta(days=7)
    day_in_a_week_string = day_in_a_week.strftime('%m/%d/%Y')
    is_next_month = today.month != day_in_a_week.month

    driver.get_screenshot_as_file('screenshots/smithgolf_mainpage_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))

    # Login
    driver.find_element_by_xpath("//a[@class='login-link popup-link']").click()
    driver.get_screenshot_as_file('screenshots/smithgolf_LoginEmpty_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))
    driver.find_element_by_id("weblogin_username").clear()
    driver.find_element_by_id("weblogin_username").send_keys(config['SmithSite']['username'])
    driver.find_element_by_id("weblogin_password").clear()
    driver.find_element_by_id("weblogin_password").send_keys(config['SmithSite']['password'])
    driver.get_screenshot_as_file('screenshots/smithgolf_LoginFilled_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))
    driver.find_element_by_xpath("//input[@id='weblogin_buttonlogin']").click()

    # Homepage - Navigate to Golf
    driver.get_screenshot_as_file('screenshots/smithgolf_HomePageLoggedIn_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))
    driver.get(config['SmithSite']['golfUrl'])
    driver.get_screenshot_as_file('screenshots/smithgolf_LoggedInGolfTeeTimes_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))

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

    driver.find_element_by_xpath(course_dropdown_xpath).click()
    driver.find_element_by_xpath(num_players_dropdown_xpath).click()
    driver.find_element_by_xpath(num_holes_dropdown_xpath).click()
    driver.find_element_by_xpath(begin_time_dropdown_xpath).click()
    driver.find_element_by_xpath(begin_time_input_xpath).click()
    driver.find_element_by_xpath(begin_date_dropdown_xpath).click()
    if(is_next_month):
        driver.find_element_by_xpath(next_month_xpath).click()
    driver.find_element_by_xpath(begin_date_input_xpath).click()
    driver.find_element_by_xpath(search_tee_times_button_xpath).click()

    driver.get_screenshot_as_file('screenshots/smithgolf_teetimes_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))

    # Getting all available TeeTimes
    tee_time_rows = driver.find_element_by_xpath(search_tee_times_table).find_elements_by_css_selector('tr')
    tee_time_cells = driver.find_element_by_xpath(search_tee_times_table).find_elements_by_css_selector('td')

    tee_times_grouped = [tee_time_cells[n: n + 8] for n in range(0, len(tee_time_cells), 8)]
    tee_times_dict = {tee_times_grouped[i][1].text : {'AddToCartIndex': str(i), 'IsAddToCartAvailable': str(tee_times_grouped[i][0].text == 'Add To Cart'), 'Date' : tee_times_grouped[i][2].text, 'Holes': tee_times_grouped[i][3].text, 'Course': tee_times_grouped[i][4].text, 'OpenSlots': tee_times_grouped[i][5].text } for i in range(len(tee_times_grouped))}
    #teeTimeRowHeaders = ['AddToCart', 'Time', 'Date', 'Holes', 'Course', 'Open', 'Slots', 'Status']

    # preferred_tee_times_in_order = ['10:30 am', '10:39 am', '10:48 am', '10:57 am', '11:06 am', '11:15 am', '11:24 am', '11:33 am', '11:42 am', '11:51 am', '12:00 pm', '12:09 pm', '12:18 pm', '12:27 pm', '12:36 pm', '12:45 pm', '12:54 pm', '1:03 pm', '1:12 pm', '1:21 pm', '1:30 pm', '1:39 pm', '1:48 pm', '1:57 pm', '10:21 am', '10:12 am', '10:03 am', '9:54 am', '9:45 am', '9:36 am', '9:27 am', '9:18 am', '9:09 am', '9:00 am', '2:06 pm', '2:15 pm', '2:24 pm', '2:33 pm', '2:42 pm', '8:51 am', '8:42 am', '8:33 am', '8:24 am', '2:51 pm', '3:00 pm', '3:09 pm', '3:18 pm', '3:27 pm', '3:36 pm', '3:45 pm', '3:54 pm', '4:03 pm', '4:12 pm', '4:21 pm', '4:30 pm', '4:39 pm', '4:48 pm', '4:57 pm', '5:06 pm', '5:15 pm', '5:24 pm', '5:33 pm', '5:42 pm', '5:51 pm', '6:00 pm', '6:09 pm', '6:18 pm', '6:27 pm', '6:36 pm', '6:45 pm', '6:54 pm']
    preferred_tee_times_in_order = ['1:30 pm', '1:39 pm', '1:48 pm', '1:57 pm', '2:06 pm', '2:15 pm', '2:24 pm', '2:33 pm', '2:42 pm', '2:51 pm', '3:00 pm', '3:09 pm', '3:18 pm', '3:27 pm', '1:12 pm', '1:21 pm', '3:36 pm', '3:45 pm', '3:54 pm', '4:03 pm', '10:30 am', '10:39 am', '10:48 am', '10:57 am', '11:06 am', '11:15 am', '11:24 am', '11:33 am', '11:42 am', '11:51 am', '12:00 pm', '12:09 pm', '12:18 pm', '12:27 pm', '12:36 pm', '12:45 pm', '12:54 pm', '1:03 pm', '10:21 am', '10:12 am', '10:03 am', '9:54 am', '9:45 am', '9:36 am', '9:27 am', '9:18 am', '9:09 am', '9:00 am', '8:51 am', '8:42 am', '8:33 am', '8:24 am', '2:51 pm', '3:00 pm', '3:09 pm', '3:18 pm', '3:27 pm', '3:36 pm', '3:45 pm', '3:54 pm', '4:03 pm', '4:12 pm', '4:21 pm', '4:30 pm', '4:39 pm', '4:48 pm', '4:57 pm', '5:06 pm', '5:15 pm', '5:24 pm', '5:33 pm', '5:42 pm', '5:51 pm', '6:00 pm', '6:09 pm', '6:18 pm', '6:27 pm', '6:36 pm', '6:45 pm', '6:54 pm']

    # Booking available tee time in order of preferred times
    for tee_time in preferred_tee_times_in_order:
        if (tee_time in tee_times_dict) and (tee_times_dict[tee_time]['IsAddToCartAvailable'] == 'True') and (tee_times_dict[tee_time]['Holes'] == '18 (Front)') and (tee_times_dict[tee_time]['Course'] == 'H. Smith Richardson Golf Course') and (tee_times_dict[tee_time]['OpenSlots'] == '4'):
            add_to_cart_elements = driver.find_elements_by_xpath("//a[@title='Add To Cart' or @title='Unavailable']")
            add_to_cart_elements[int(tee_times_dict[tee_time]['AddToCartIndex'])].click()

            driver.get_screenshot_as_file('screenshots/smithgolf_personalinfo_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))
            driver.find_element_by_id('golfmemberselection_buttononeclicktofinish').click()

            # Successfully booked Tee Time
            try:
                time.sleep(5)
                driver.get_screenshot_as_file('screenshots/smithgolf_submitted_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))
                driver.find_element_by_id('webconfirmation_pageheader')
                success_message = "{} {} - booked {} teetime on {} at {} for {} people.".format(today.strftime('%m/%d/%Y'), datetime.now().strftime("%H:%M:%S"), tee_time, tee_times_dict[tee_time]['Date'], tee_times_dict[tee_time]['Course'], tee_times_dict[tee_time]['OpenSlots'])
                print(success_message)
                send_sms(success_message)
                break
            # Failed to book Tee Time, retrying with next preferred time
            except:
                driver.get_screenshot_as_file('screenshots/smithgolf_failed_{}-{}.png'.format(today.strftime('%m.%d.%Y'), time_to_run.replace(':', '-')))
                fail_message = "{} {} - FAILED to book {} teetime on {} at {} for {} people. Trying next preferred teetime...".format(today.strftime('%m/%d/%Y'), datetime.now().strftime("%H:%M:%S"), tee_time, tee_times_dict[tee_time]['Date'], tee_times_dict[tee_time]['Course'], tee_times_dict[tee_time]['OpenSlots'])
                print(fail_message)
                send_sms(fail_message, True)
                driver.back()
                time.sleep(1)
    
    print("\nQuitting...")
    driver.quit()

if __name__ == "__main__":
    schedule.every().day.at(time_to_run).do(get_smith_tee_times)
    while True:
        schedule.run_pending()
        print(str(datetime.now()) + ": not " + time_to_run + " yet! Checking again later...")
        time.sleep(5)