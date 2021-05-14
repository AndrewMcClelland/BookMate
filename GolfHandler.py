import platform
import requests
from datetime import datetime

from TwilioHandler import TwilioHandler

class GolfHandler:
    def __init__(self, numberHoles: str, numberPlayers: str, preferredTeeTimeRanges: str, baseUrl: str, bookTimeEnabled: bool, twilioHandler: TwilioHandler, logger):
        self.numberHoles = numberHoles
        self.numberPlayers = numberPlayers
        self.preferredTeeTimeRanges = preferredTeeTimeRanges.split(',')
        self.baseUrl = baseUrl
        self.bookTimeEnabled = bookTimeEnabled
        self.twilioHandler = twilioHandler
        self.logger = logger

        self.session = requests.Session()
    
    def _SortAvailableTeeTimes(self, availableTeeTimes):
        sortedAvailableTeeTimes = []
        sortedAvailableTeeTimeBuckets = {}
        preferredTeeTimeBuckets = {}
        timePriority = 1

        teeTimeFormat = "%I:%M %p"

        # Create dictionary for each preferred tee time range
        for preferredTeeTimeRange in self.preferredTeeTimeRanges:
            preferredFirstTime = datetime.strptime(preferredTeeTimeRange.split('-')[0], teeTimeFormat)
            preferredLastTime = datetime.strptime(preferredTeeTimeRange.split('-')[1], teeTimeFormat)
            earlierPreferredTime = min(preferredFirstTime, preferredLastTime)
            laterPreferredTime = max(preferredFirstTime, preferredLastTime)
            ascendingTimePreference = preferredFirstTime < preferredLastTime

            preferredTeeTimeBuckets[timePriority] = {
                'preferredFirstTime': preferredFirstTime,
                'preferredLastTime': preferredLastTime,
                'earlierPreferredTime': earlierPreferredTime,
                'laterPreferredTime': laterPreferredTime,
                'ascendingTimePreference': ascendingTimePreference,
            }

            sortedAvailableTeeTimeBuckets[timePriority] = []

            timePriority += 1

        for availableTeeTime in availableTeeTimes:
            # Convert string to datetime object
            teeTime = datetime.strptime(availableTeeTime, teeTimeFormat)

            # Assign available tee time to preference bucket based on preferred tee time ranges
            for priorityKey in preferredTeeTimeBuckets:
                if preferredTeeTimeBuckets[priorityKey]['earlierPreferredTime'] <= teeTime and teeTime <= preferredTeeTimeBuckets[priorityKey]['laterPreferredTime']:
                    sortedAvailableTeeTimeBuckets[priorityKey].append(teeTime)
        
        # To remove padded leading 0 on datetime: use '#' for Windows and '-' for Linux
        isWindows = platform.system() == "Windows"
        printTeeTimeFormat = "%{0}I:%M %p".format("#" if isWindows else '-')
        # In order of tee time range priority, sort each bucket of tee times (order specified by 'ascendingTimePreference') and append to returned 'sortedAvailableTeeTimes' list
        for priority in range(1, timePriority):
            currPriorityTeeTimes = sortedAvailableTeeTimeBuckets[priority]
            sortedPriorityTeeTimes = sorted(currPriorityTeeTimes, reverse=not preferredTeeTimeBuckets[priority]['ascendingTimePreference'])
            sortedAvailableTeeTimes.extend([teeTime.strftime(printTeeTimeFormat).lower() for teeTime in sortedPriorityTeeTimes])

        return sortedAvailableTeeTimes
    
    def BookTeeTimes(self):
        raise NotImplementedError()