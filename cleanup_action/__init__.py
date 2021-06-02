import datetime
import logging
import os
import uuid
import json
import azure.functions as func
from azure.appconfiguration import AzureAppConfigurationClient
from opencensus.ext.azure.log_exporter import AzureLogHandler
from opencensus.trace import config_integration
from opencensus.trace.samplers import AlwaysOnSampler
from opencensus.trace.tracer import Tracer

from handlers.smithgolf_handler import SmithGolfHandler
from handlers.twilio_handler import TwilioHandler
from models.booking_model import BookingModel
from adapters.tablestorage_adapter import TableStorageAdapter
from services.booking_tablestorage_service import BookingTableStorageService
from models.bookerworkload import BookerWorkload

def main(message: func.ServiceBusMessage):

    config_integration.trace_integrations(['logging'])
    logging.basicConfig(format='%(asctime)s traceId=%(traceId)s spanId=%(spanId)s %(message)s')
    tracer = Tracer(sampler=AlwaysOnSampler())

    logger = logging.getLogger(__name__)
    logger.addHandler(AzureLogHandler(
        connection_string=os.environ["APPLICATIONINSIGHTS_CONNECTION_STRING"])
    )

    message_body = message.get_body().decode("utf-8")
    message_body_dict = json.loads(message_body)
    message_booking_model = BookingModel(**message_body_dict)
    message_booking_model.booker_workload = BookerWorkload(message_body_dict['booker_workload'])

    logger.info("CleanupAction_Start")

    table_storage_adapter = TableStorageAdapter(account_name=os.environ["AzureStorage_AccountName"],
                                              account_key=os.environ["AzureStorage_AccountKey"])

    booking_table_storage_service = BookingTableStorageService(table_storage_adapter)

    try:
        # If booking model is set to run only once, remove it from the storage table
        if not message_booking_model.is_repetitive:
            booking_table_storage_service.delete_booking_entity(message_booking_model)
        # Else reset the 'is_scheduled_to_run flag' back to false
        else:
            booking_table_storage_service.set_unscheduled_entity(message_booking_model)

    except Exception as e:
        logger.exception(f"CleanupAction_Error : {e}")

    logger.info("CleanupAction_End")
