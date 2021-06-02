from azure.cosmosdb.table.models import Entity

class BookingEntity(Entity):
    def __init__(self, booker_workload: str, username: str, cron_schedule: str, is_repetitive: bool, preferred_times: str, days_to_book_in_advance: int, number_players: int, number_holes: int, is_enabled: bool, is_next_run_scheduled: bool):
        super().__init__({"PartitionKey": booker_workload, "RowKey": username})
        self.booker_workload = booker_workload
        self.username = username
        self.cron_schedule = cron_schedule
        self.is_repetitive = is_repetitive
        self.preferred_times = preferred_times
        self.days_to_book_in_advance = days_to_book_in_advance
        self.number_players = number_players
        self.number_holes = number_holes
        self.is_enabled = is_enabled
        self.is_next_run_scheduled = is_next_run_scheduled
