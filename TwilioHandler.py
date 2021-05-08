from datetime import datetime
from twilio.rest import Client

class TwilioHandler:
    def __init__(self, accountSID, authToken, toNumber, fromNumber):
        self.accountSID = accountSID
        self.authToken = authToken
        self.toNumber = fromNumber
        self.fromNumber = fromNumber
    
    def send_sms(self, body):
        client = Client(self.accountSID, self.authToken) 
        
        maxBodyChars = 1500
        messageBodies = [body[i: i + maxBodyChars] for i in range(0, len(body), maxBodyChars)]

        for messageBody in messageBodies:
            message = client.messages.create( 
                                    from_=self.fromNumber,
                                    body=messageBody,      
                                    to=self.toNumber
                                    )
            
        print("Sent SMS at " + datetime.now().strftime("%m/%d/%Y, %H:%M:%S") + "\t Message:" + message.sid)