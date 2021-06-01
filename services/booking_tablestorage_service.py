from typing import List

from adapters.tablestorage_adapter import TableStorageAdapter
from models.bookerworkload import BookerWorkload
from models.booking_model import BookingModel
from models.tableentities.booking_entity import BookingEntity

class BookingTableStorageService:
    def __init__(self, booking_table_repository: TableStorageAdapter) -> None:
        self.booking_table_repository = booking_table_repository
        self.table_name = "BookingEntity"

    def get_booking_entity(self, booker_workload: BookerWorkload, username: str) -> BookingModel:
        entity = self.booking_table_repository.get_entity(self.table_name, booker_workload.name, username)

        return self.__convert_entity_to_model__(entity)

    def get_booking_entities(self) -> List[BookingModel]:
        entities = self.booking_table_repository.get_all_entities(self.table_name)

        return self.__convert_entities_to_model__(entities)

    def get_enabled_booking_entities(self) -> List[BookingModel]:
        enabled_entities_filter = "is_enabled eq 'true'"
        entities = self.booking_table_repository.get_all_entities(self.table_name, enabled_entities_filter)

        return self.__convert_entities_to_model__(entities)

    def __convert_entity_to_model__(self, entity: BookingEntity) -> List[BookingModel]:
        return BookingModel(booker_workload=BookerWorkload[entity.booker_workload],
                            username=entity.username,
                            cron_schedule=entity.cron_schedule,
                            is_repetitive=entity.is_repetitive,
                            preferred_times=entity.preferred_times,
                            days_to_book_in_advance=entity.days_to_book_in_advance.value,
                            number_players=entity.number_players.value,
                            number_holes=entity.number_holes.value,
                            is_enabled=entity.is_enabled,
                            is_next_run_schedlued=entity.is_next_run_schedlued )

    def __convert_entities_to_model__(self, entities: List[BookingEntity]) -> List[BookingModel]:
        models = []
        for entity in entities:
            models.append(self.__convert_entity_to_model__(entity))

        return models
