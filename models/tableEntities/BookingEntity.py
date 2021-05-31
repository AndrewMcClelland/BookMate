from azure.cosmosdb.table.models import Entity

class BookingEntity(Entity):
    def __init__(self, bookerWorkload: str, username: str, cronSchedule: str, isRepetitive: bool, preferredTimes: str, daysToBookInAdvance: int, numberPlayers: int, numberHoles: int):
        self.BookerWorkload = bookerWorkload
        self.Username = username
        self.CronSchedule = cronSchedule
        self.IsRepetitive = isRepetitive
        self.PreferredTimes = preferredTimes
        self.DaysToBookInAdvance = daysToBookInAdvance
        self.NumberPlayers = numberPlayers
        self.NumberHoles = numberHoles