import logging
from datetime import datetime
from twilio.rest import Client

class TwilioHandler:
    def __init__(self, account_sid, auth_token, to_numbers, from_number, logger):
        self.account_sid = account_sid
        self.auth_token = auth_token
        self.to_numbers = to_numbers
        self.from_number = from_number
        self.logger = logger
    
    def send_sms(self, body, send_to_me_only = False):

        self.logger.info("TwilioHandler.sendSms_Start")
        
        to_numbers = [self.to_numbers.split(',')[0]] if send_to_me_only else self.to_numbers.split(',')

        client = Client(self.account_sid, self.auth_token) 
        
        max_body_chars = 1500
        message_bodies = [body[i: i + max_body_chars] for i in range(0, len(body), max_body_chars)]

        for to_number in to_numbers:
            for message_body in message_bodies:
                message = client.messages.create( 
                                        from_=self.from_number,
                                        body=message_body,      
                                        to=to_number
                                        )
                
                self.logger.info("TwilioHandler.sendSms_Start", extra={'custom_dimensions': {'SmsSendTime': datetime.now().strftime("%H:%M:%S on %m/%d/%Y"), 'SmsTo': str(message.to), 'SmsSID': message.sid}})
        
        self.logger.info("TwilioHandler.sendSms_End")