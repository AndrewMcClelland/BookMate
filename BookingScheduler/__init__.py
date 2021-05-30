import datetime
import logging
import os
import uuid
import azure.functions as func
from opencensus.ext.azure.log_exporter import AzureLogHandler
from opencensus.trace import config_integration
from opencensus.trace.samplers import AlwaysOnSampler
from opencensus.trace.tracer import Tracer
from datetime import datetime
from croniter import croniter

from adapters.TableStorageAdapter import TableStorageAdapter
from adapters.ServiceBusTopicAdapter import ServiceBusTopicAdapter
from services.BookingTableStorageService import BookingTableStorageService
from models.BookerWorkload import BookerWorkload
from models.tableEntities.BookingEntity import BookingEntity

def main(mytimer: func.TimerRequest) -> None:

    config_integration.trace_integrations(['logging'])
    logging.basicConfig(format='%(asctime)s traceId=%(traceId)s spanId=%(spanId)s %(message)s')
    tracer = Tracer(sampler=AlwaysOnSampler())

    logger = logging.getLogger(__name__)
    logger.addHandler(AzureLogHandler(
        connection_string=os.environ["APPLICATIONINSIGHTS_CONNECTION_STRING"])
    )

    logger.info("BookingScheduler_Start")

    tableStorageAdapter = TableStorageAdapter(accountName=os.environ["AzureStorage_AccountName"],
                                              accountKey=os.environ["AzureStorage_AccountKey"])

    bookingTableStorageService = BookingTableStorageService(tableStorageAdapter)

    bookingTopic = ServiceBusTopicAdapter(connectionString=os.environ["AzureServiceBus_BookingTopic_ConnectionString_Send"],
                                          topicName="bookingtopic")

    try:
        bookingEntities = bookingTableStorageService.GetBookingEntities()

        # HOW TO ENSURE THAT DUPLICATE BOOKINGS ARENT ENQUEUED FOR THE SAME TIME???????????????????
        for bookingEntity in bookingEntities:
            messageProperties ={"BookerWorkload": bookingEntity.BookerWorkload.name}
            cron = croniter(bookingEntity.CronSchedule)
            enqueueTime = cron.get_next(datetime)
            bookingTopic.SendMessage(bookingEntity, messageProperties, None)

    except Exception as e:
        logger.exception(f"BookingScheduler_Error : {e}")

    logger.info("BookingScheduler_End")
