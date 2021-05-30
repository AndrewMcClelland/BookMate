from models.BookerWorkload import BookerWorkload

class BookingModel:
    def __init__(self, bookerWorkload: BookerWorkload, username: str, cronSchedule: str, isReptitive: bool, preferredTimes: str, daysToBookInAdvance: int, numberPlayers: int, numberHoles: int):
        super().__init__()

        self.BookerWorkload = bookerWorkload
        self.Username = username
        self.CronSchedule = cronSchedule
        self.IsRepetitive = isReptitive
        self.PreferredTimes = preferredTimes
        self.DaysToBookInAdvance = daysToBookInAdvance
        self.NumberPlayers = numberPlayers
        self.NumberHoles = numberHoles