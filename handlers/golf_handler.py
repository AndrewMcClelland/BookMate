import platform
import requests
from datetime import datetime, timedelta

from handlers.twilio_handler import TwilioHandler

class GolfHandler:
    def __init__(self, number_holes: str, number_players: str, preferred_tee_time_ranges: str, days_to_book_in_advance: str, base_url: str, book_time_enabled: bool, twilio_handler: TwilioHandler, logger):
        self.number_holes = number_holes
        self.number_players = number_players
        self.preferred_tee_time_ranges = preferred_tee_time_ranges.split(',')
        self.base_url = base_url
        self.book_time_enabled = book_time_enabled
        self.twilio_handler = twilio_handler
        self.logger = logger

        # Book for date 'days_to_book_in_advance' days from today
        today = datetime.now()
        self.date_to_book = today + timedelta(days=int(days_to_book_in_advance))

        self.session = requests.Session()

    def _sort_available_tee_times(self, available_tee_times):
        if not available_tee_times:
            return []

        sorted_available_tee_times = []
        sorted_available_tee_time_buckets = {}
        preferred_tee_time_buckets = {}
        time_priority = 1

        is_upper_case_time_meridian = available_tee_times[0].isupper()

        tee_time_format = "%I:%M %p"

        # Create dictionary for each preferred tee time range
        for preferred_tee_time_range in self.preferred_tee_time_ranges:
            preferred_first_time = datetime.strptime(preferred_tee_time_range.split('-')[0], tee_time_format)
            preferred_last_time = datetime.strptime(preferred_tee_time_range.split('-')[1], tee_time_format)
            earlier_preferred_time = min(preferred_first_time, preferred_last_time)
            later_preferred_time = max(preferred_first_time, preferred_last_time)
            ascending_time_preference = preferred_first_time < preferred_last_time

            preferred_tee_time_buckets[time_priority] = {
                'preferred_first_time': preferred_first_time,
                'preferred_last_time': preferred_last_time,
                'earlier_preferred_time': earlier_preferred_time,
                'later_preferred_time': later_preferred_time,
                'ascending_time_preference': ascending_time_preference,
            }

            sorted_available_tee_time_buckets[time_priority] = []

            time_priority += 1

        for available_tee_time in available_tee_times:
            # Convert string to datetime object
            tee_time = datetime.strptime(available_tee_time, tee_time_format)

            # Assign available tee time to preference bucket based on preferred tee time ranges
            for priority_key in preferred_tee_time_buckets:
                if preferred_tee_time_buckets[priority_key]['earlier_preferred_time'] <= tee_time and tee_time <= preferred_tee_time_buckets[priority_key]['later_preferred_time']:
                    sorted_available_tee_time_buckets[priority_key].append(tee_time)

        # To remove padded leading 0 on datetime: use '#' for Windows and '-' for Linux
        is_windows = platform.system() == "Windows"
        print_tee_time_format = "%{0}I:%M %p".format("#" if is_windows else '-')
        # In order of tee time range priority, sort each bucket of tee times (order specified by 'ascending_time_preference') and append to returned 'sorted_available_tee_times' list
        for priority in range(1, time_priority):
            curr_priority_tee_times = sorted_available_tee_time_buckets[priority]
            sorted_priority_tee_times = sorted(curr_priority_tee_times, reverse=not preferred_tee_time_buckets[priority]['ascending_time_preference'])
            sorted_available_tee_times.extend([tee_time.strftime(print_tee_time_format).upper() if is_upper_case_time_meridian else tee_time.strftime(print_tee_time_format).lower() for tee_time in sorted_priority_tee_times])

        return sorted_available_tee_times

    def book_tee_times(self):
        raise NotImplementedError()
