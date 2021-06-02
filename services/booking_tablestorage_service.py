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

        return self.__convert_entities_to_models__(entities)

    def get_enabled_unscheduled_booking_entities(self) -> List[BookingModel]:
        enabled_entities_filter = "is_enabled eq true and is_next_run_scheduled eq false"
        entities = self.booking_table_repository.get_all_entities(self.table_name, enabled_entities_filter)

        return self.__convert_entities_to_models__(entities)

    def set_unscheduled_entity(self, model: BookingModel) -> None:
        entity = self.__convert_model_to_entity__(model)
        entity.is_next_run_scheduled = False

        self.booking_table_repository.insert_entity(self.table_name, entity)

    def set_scheduled_entities(self, models: List[BookingModel]) -> None:
        entities = self.__convert_models_to_entities__(models)

        for entity in entities:
            entity.is_next_run_scheduled = True

        self.booking_table_repository.insert_entities(self.table_name, entities)

    def delete_booking_entity(self, model: BookingModel) -> None:
        entity = self.__convert_model_to_entity__(model)
        self.booking_table_repository.delete_entity(self.table_name, entity)

    def __convert_entity_to_model__(self, entity: BookingEntity) -> BookingModel:
        return BookingModel(booker_workload=BookerWorkload(entity.booker_workload),
                            username=entity.username,
                            cron_schedule=entity.cron_schedule,
                            is_repetitive=entity.is_repetitive,
                            preferred_times=entity.preferred_times,
                            days_to_book_in_advance=entity.days_to_book_in_advance,
                            number_players=entity.number_players,
                            number_holes=entity.number_holes,
                            is_enabled=entity.is_enabled,
                            is_next_run_scheduled=entity.is_next_run_scheduled )

    def __convert_entities_to_models__(self, entities: List[BookingEntity]) -> List[BookingModel]:
        models = []
        for entity in entities:
            models.append(self.__convert_entity_to_model__(entity))

        return models

    def __convert_model_to_entity__(self, model: BookingModel) -> BookingEntity:
        return BookingEntity(booker_workload=model.booker_workload.value,
                            username=model.username,
                            cron_schedule=model.cron_schedule,
                            is_repetitive=model.is_repetitive,
                            preferred_times=model.preferred_times,
                            days_to_book_in_advance=model.days_to_book_in_advance,
                            number_players=model.number_players,
                            number_holes=model.number_holes,
                            is_enabled=model.is_enabled,
                            is_next_run_scheduled=model.is_next_run_scheduled )

    def __convert_models_to_entities__(self, models: List[BookingModel]) -> List[BookingEntity]:
        entities = []
        for model in models:
            entities.append(self.__convert_model_to_entity__(model))

        return entities
