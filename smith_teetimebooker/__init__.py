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
from models.bookerworkload import BookerWorkload
from adapters.servicebustopic_adapter import ServiceBusTopicAdapter

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

    # Setup IAM for FunctionApp - https://docs.microsoft.com/en-us/azure/azure-app-configuration/howto-integrate-azure-managed-service-identity?tabs=core2x
    app_config_client = AzureAppConfigurationClient.from_connection_string(os.getenv('AppConfigConnectionString'))

    # Get Feature Flag
    book_time_enabled = app_config_client.get_configuration_setting(key=".appconfig.featureflag/BookTeeTime", label="prod")

    logger.info("SmithTeeTimeBooker_Start")

    twilio_handler = TwilioHandler(account_sid=os.environ["Twilio_AccountSID"],
                                  auth_token=os.environ["Twilio_AuthToken"],
                                  to_numbers=os.environ["Twilio_ToNumbers"],
                                  from_number=os.environ["Twilio_FromNumber"],
                                  logger=logger)

    smith_golf_handler = SmithGolfHandler(number_holes=message_booking_model.number_holes,
                                        number_players=message_booking_model.number_players,
                                        preferred_tee_time_ranges=message_booking_model.preferred_times,
                                        days_to_book_in_advance=message_booking_model.days_to_book_in_advance,
                                        username=os.environ["Smith_Username"],
                                        password=os.environ["Smith_Password"],
                                        player_identifier=os.environ["Smith_PlayerIdentifier"],
                                        base_url=os.environ["Smith_Url_Base"],
                                        login_endpoint=os.environ["Smith_Endpoint_Login"],
                                        search_times_endpoint=os.environ["Smith_Endpoint_SearchTimes"],
                                        submit_cart_endpoint=os.environ["Smith_Endpoint_SubmitCart"],
                                        book_time_enabled=os.environ["Smith_BookTeeTime"] == "true",
                                        twilio_handler=twilio_handler,
                                        logger=logger)

    actions_topic_adapter = ServiceBusTopicAdapter(connection_string=os.environ["AzureServiceBus_ActionsTopic_ConnectionString_Send"],
                                          topic_name="actionstopic")

    try:
        smith_golf_handler.book_tee_times()

        message_properties = {"Action": "Cleanup"}
        actions_topic_adapter.send_message(message_booking_model, message_properties)

    except Exception as e:
        logger.exception(f"SmithTeeTimeBooker_Error : {e}")

    logger.info("SmithTeeTimeBooker_End")
