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

from adapters.tablestorage_adapter import TableStorageAdapter
from adapters.servicebustopic_adapter import ServiceBusTopicAdapter
from services.booking_tablestorage_service import BookingTableStorageService
from models.bookerworkload import BookerWorkload
from models.tableentities.booking_entity import BookingEntity

def main(mytimer: func.TimerRequest) -> None:

    config_integration.trace_integrations(['logging'])
    logging.basicConfig(format='%(asctime)s traceId=%(traceId)s spanId=%(spanId)s %(message)s')
    tracer = Tracer(sampler=AlwaysOnSampler())

    logger = logging.getLogger(__name__)
    logger.addHandler(AzureLogHandler(
        connection_string=os.environ["APPLICATIONINSIGHTS_CONNECTION_STRING"])
    )

    logger.info("BookingScheduler_Start")

    table_storage_adapter = TableStorageAdapter(account_name=os.environ["AzureStorage_AccountName"],
                                              account_key=os.environ["AzureStorage_AccountKey"])

    booking_table_storage_service = BookingTableStorageService(table_storage_adapter)

    booking_topic = ServiceBusTopicAdapter(connection_string=os.environ["AzureServiceBus_BookingTopic_ConnectionString_Send"],
                                          topic_name="bookingtopic")

    try:
        booking_entities = booking_table_storage_service.get_booking_entities()

        # HOW TO ENSURE THAT DUPLICATE BOOKINGS ARENT ENQUEUED FOR THE SAME TIME???????????????????
        # Service Bus Duplicate Detection Time Window???
        for booking_entity in booking_entities:
            message_properties = {"BookerWorkload": booking_entity.booker_workload.name}
            cron = croniter(booking_entity.cron_schedule)
            enqueue_time = cron.get_next(datetime)
            booking_topic.send_message(booking_entity, message_properties, enqueue_time)

    except Exception as e:
        logger.exception(f"BookingScheduler_Error : {e}")

    logger.info("BookingScheduler_End")
