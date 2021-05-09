from datetime import datetime
from twilio.rest import Client

class TwilioHandler:
    def __init__(self, accountSID, authToken, toNumbers, fromNumber):
        self.accountSID = accountSID
        self.authToken = authToken
        self.toNumbers = toNumbers
        self.fromNumber = fromNumber
    
    def sendSms(self, body, sendToMeOnly = False):

        to_numbers = [self.toNumbers.split(',')[0]] if sendToMeOnly else self.toNumbers.split(',')

        client = Client(self.accountSID, self.authToken) 
        
        maxBodyChars = 1500
        messageBodies = [body[i: i + maxBodyChars] for i in range(0, len(body), maxBodyChars)]

        for to_number in self.toNumbers:
            for messageBody in messageBodies:
                message = client.messages.create( 
                                        from_=self.fromNumber,
                                        body=messageBody,      
                                        to=to_number
                                        )
                
                print("Sent SMS at {} - To: {}\tSID: {}".format(datetime.now().strftime("%H:%M:%S on %m/%d/%Y"), str(message.to), message.sid))