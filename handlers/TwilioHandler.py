import logging
from datetime import datetime
from twilio.rest import Client

class TwilioHandler:
    def __init__(self, accountSID, authToken, toNumbers, fromNumber, logger):
        self.accountSID = accountSID
        self.authToken = authToken
        self.toNumbers = toNumbers
        self.fromNumber = fromNumber
        self.logger = logger
    
    def sendSms(self, body, sendToMeOnly = False):

        self.logger.info("TwilioHandler.sendSms_Start")
        
        to_numbers = [self.toNumbers.split(',')[0]] if sendToMeOnly else self.toNumbers.split(',')

        client = Client(self.accountSID, self.authToken) 
        
        maxBodyChars = 1500
        messageBodies = [body[i: i + maxBodyChars] for i in range(0, len(body), maxBodyChars)]

        for to_number in to_numbers:
            for messageBody in messageBodies:
                message = client.messages.create( 
                                        from_=self.fromNumber,
                                        body=messageBody,      
                                        to=to_number
                                        )
                
                self.logger.info("TwilioHandler.sendSms_Start", extra={'custom_dimensions': {'SmsSendTime': datetime.now().strftime("%H:%M:%S on %m/%d/%Y"), 'SmsTo': str(message.to), 'SmsSID': message.sid}})
        
        self.logger.info("TwilioHandler.sendSms_End")