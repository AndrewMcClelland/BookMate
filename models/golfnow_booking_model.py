from models.booking_model import BookingModel

class GolfNowBookingModel(BookingModel):
    def __init__(self, booker_workload, username, cron_schedule, is_repetitive, preferred_times, days_to_book_in_advance, number_players, number_holes, course_id):
        super().__init__(booker_workload, username, cron_schedule, is_repetitive, preferred_times, days_to_book_in_advance, number_players, number_holes)
        
        self.course_id = course_id