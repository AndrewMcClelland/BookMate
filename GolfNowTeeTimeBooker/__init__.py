import datetime
import logging
import os
import uuid
import azure.functions as func
from azure.appconfiguration import AzureAppConfigurationClient
from opencensus.ext.azure.log_exporter import AzureLogHandler
from opencensus.trace import config_integration
from opencensus.trace.samplers import AlwaysOnSampler
from opencensus.trace.tracer import Tracer

from handlers.GolfNowGolfHandler import GolfNowGolfHandler
from handlers.TwilioHandler import TwilioHandler

def main(mytimer: func.TimerRequest) -> None:

    config_integration.trace_integrations(['logging'])
    logging.basicConfig(format='%(asctime)s traceId=%(traceId)s spanId=%(spanId)s %(message)s')
    tracer = Tracer(sampler=AlwaysOnSampler())

    logger = logging.getLogger(__name__)
    logger.addHandler(AzureLogHandler(
        connection_string=os.environ["APPLICATIONINSIGHTS_CONNECTION_STRING"])
    )

    # Setup IAM for FunctionApp - https://docs.microsoft.com/en-us/azure/azure-app-configuration/howto-integrate-azure-managed-service-identity?tabs=core2x
    appConfigClient = AzureAppConfigurationClient.from_connection_string(os.getenv('AppConfigConnectionString'))
    numberHoles = appConfigClient.get_configuration_setting(key="GolfNow:NumberHoles", label="prod").value
    numberPlayers = appConfigClient.get_configuration_setting(key="GolfNow:NumberPlayers", label="prod").value
    preferredTeeTimeRanges = appConfigClient.get_configuration_setting(key="GolfNow:PreferredTeeTimeRanges", label="prod").value

    # Get Feature Flag
    # enableBookingTeeTime = ???

    logger.info("GolfNowTeeTimeBooker_Start")
    
    twilioHandler = TwilioHandler(accountSID=os.environ["Twilio_AccountSID"],
                                  authToken=os.environ["Twilio_AuthToken"],
                                  toNumbers=os.environ["Twilio_ToNumbers"],
                                  fromNumber=os.environ["Twilio_FromNumber"],
                                  logger=logger)

    golfNowGolfHandler = GolfNowGolfHandler(numberHoles=numberHoles,
                                        numberPlayers=numberPlayers,
                                        preferredTeeTimeRanges=preferredTeeTimeRanges,
                                        username=os.environ["Smith_Username"],
                                        password=os.environ["Smith_Password"],
                                        baseUrl=os.environ["Smith_Url_Base"],
                                        bookTimeEnabled=os.environ["Smith_BookTeeTime"] == "true",
                                        twilioHandler=twilioHandler,
                                        logger=logger)

    try:
        golfNowGolfHandler.BookTeeTimes()
    except Exception as e:
        logger.exception(f"GolfNowTeeTimeBooker_Error : {e}")

    logger.info("GolfNowTeeTimeBooker_End")
