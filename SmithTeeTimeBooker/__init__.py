import datetime
import logging
import os

import azure.functions as func

from SmithGolfHandler import SmithGolfHandler
from TwilioHandler import TwilioHandler


def main(mytimer: func.TimerRequest) -> None:
    print("SmithTeeTimeBooker function ran at %s", datetime.datetime.now())
    
    twilioHandler = TwilioHandler(os.environ["Twilio_AccountSID"],
                                  os.environ["Twilio_AuthToken"],
                                  os.environ["Twilio_ToNumbers"],
                                  os.environ["Twilio_FromNumber"])

    smithGolfHandler = SmithGolfHandler(os.environ["Smith_Url"],
                                        os.environ["Smith_PreferredTeeTimes"],
                                        os.environ["Smith_Username"],
                                        os.environ["Smith_Password"],
                                        twilioHandler)

    smithGolfHandler.BookSmithTeeTimes()

    print("SmithTeeTimeBooker function completed at %s", datetime.datetime.now())
