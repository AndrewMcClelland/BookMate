from handlers.GolfHandler import GolfHandler
from handlers.TwilioHandler import TwilioHandler

class GolfNowGolfHandler(GolfHandler):
    def __init__(self, username: str, password: str, numberHoles: str, numberPlayers: str, preferredTeeTimeRanges: str, daysToBookInAdvance: str, baseUrl: str, bookTimeEnabled: bool, twilioHandler: TwilioHandler, logger):
        super().__init__(numberHoles=numberHoles, numberPlayers=numberPlayers, preferredTeeTimeRanges=preferredTeeTimeRanges, daysToBookInAdvance=daysToBookInAdvance, baseUrl=baseUrl, bookTimeEnabled=bookTimeEnabled, twilioHandler=twilioHandler, logger=logger)

        self.username = username
        self.password = password
    
    def BookTeeTimes(self):
        print("Book GolfNow")