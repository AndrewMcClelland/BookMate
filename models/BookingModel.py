from models.BookerWorkload import BookerWorkload

class BookingModel:
    def __init__(self, bookerWorkload: BookerWorkload, username: str, cronSchedule: str, isRepetitive: bool, preferredTimes: str, daysToBookInAdvance: int, numberPlayers: int, numberHoles: int):
        self.bookerWorkload = bookerWorkload
        self.username = username
        self.cronSchedule = cronSchedule
        self.isRepetitive = isRepetitive
        self.preferredTimes = preferredTimes
        self.daysToBookInAdvance = daysToBookInAdvance
        self.numberPlayers = numberPlayers
        self.numberHoles = numberHoles