from azure.cosmosdb.table.models import Entity

class BookingEntity(Entity):
    def __init__(self, bookerWorkload: str, username: str, cronSchedule: str, isReptitive: bool, preferredTimes: str, daysToBookInAdvance: int, numberPlayers: int, numberHoles: int):
        super().__init__()

        self.BookerWorkload = bookerWorkload
        self.Username = username
        self.CronSchedule = cronSchedule
        self.IsRepetitive = isReptitive
        self.PreferredTimes = preferredTimes
        self.DaysToBookInAdvance = daysToBookInAdvance
        self.NumberPlayers = numberPlayers
        self.NumberHoles = numberHoles