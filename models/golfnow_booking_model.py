from models.booking_model import BookingModel
from models.bookerworkload import BookerWorkload

class GolfNowBookingModel(BookingModel):
    def __init__(self, booker_workload: BookerWorkload, username: str, cron_schedule: str, is_repetitive: bool, preferred_times: str, days_to_book_in_advance: int, number_players: int, number_holes: int, is_enabled: bool, is_next_run_scheduled: bool, course_id: str):
        super().__init__(booker_workload=booker_workload, username=username, cron_schedule=cron_schedule, is_repetitive=is_repetitive, preferred_times=preferred_times, days_to_book_in_advance=days_to_book_in_advance, number_players=number_players, number_holes=number_holes, is_enabled=is_enabled, is_next_run_scheduled=is_next_run_scheduled)

        self.course_id = course_id
