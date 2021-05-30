from typing import TypeVar, List
from azure.cosmosdb.table.tableservice import TableService

from adapters.TableStorageAdapter import TableStorageAdapter
from models.BookerWorkload import BookerWorkload
from models.BookingModel import BookingModel

class BookingTableStorageService:
    def __init__(self, bookingTableRepository: TableStorageAdapter) -> None:
        self.bookingTableRepository = bookingTableRepository
        self.tableName = "BookingEntity"
    
    def GetBookingEntity(self, bookerWorkload: BookerWorkload, username: str) -> BookingModel:
        entity = self.bookingTableRepository.GetEntity(self.tableName, bookerWorkload.name, username)

        return BookingModel(bookerWorkload=BookerWorkload[entity.BookerWorkload],
                            username=entity.username,
                            cronSchedule=entity.CronSchedule,
                            isRepetitive=entity.IsRepetitive,
                            preferredTimes=entity.PreferredTimes,
                            daysToBookInAdvance=entity.DaysToBookInAdvance.value,
                            numberPlayers=entity.NumberPlayers.value,
                            numberHoles=entity.NumberHoles.value)
    
    def GetBookingEntities(self) -> List[BookingModel]:
        entities = self.bookingTableRepository.GetEntities(self.tableName)

        return [BookingModel(bookerWorkload=BookerWorkload[entity.BookerWorkload],
                            username=entity.Username,
                            cronSchedule=entity.CronSchedule,
                            isRepetitive=entity.IsRepetitive,
                            preferredTimes=entity.PreferredTimes,
                            daysToBookInAdvance=entity.DaysToBookInAdvance.value,
                            numberPlayers=entity.NumberPlayers.value,
                            numberHoles=entity.NumberHoles.value) for entity in entities]