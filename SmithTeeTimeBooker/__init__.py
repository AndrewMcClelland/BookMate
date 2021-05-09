import datetime
import logging
import os
import uuid

import azure.functions as func
from opencensus.ext.azure.log_exporter import AzureLogHandler
from opencensus.trace import config_integration
from opencensus.trace.samplers import AlwaysOnSampler
from opencensus.trace.tracer import Tracer

from SmithGolfHandler import SmithGolfHandler
from TwilioHandler import TwilioHandler


def main(mytimer: func.TimerRequest) -> None:

    config_integration.trace_integrations(['logging'])
    logging.basicConfig(format='%(asctime)s traceId=%(traceId)s spanId=%(spanId)s %(message)s')
    tracer = Tracer(sampler=AlwaysOnSampler())

    logger = logging.getLogger(__name__)
    logger.addHandler(AzureLogHandler(
        connection_string='InstrumentationKey={};IngestionEndpoint={}'.format(os.environ["APPINSIGHTS_INSTRUMENTATIONKEY"], os.environ["APPINSIGHTS_INGESTIONENDPOINT"]))
    )

    # logEvent = {
    #     'custom_dimensions': {
    #         'OperationId': str(uuid.uuid4),
    #         'EventName': 'SmithTeeTimeBooker_Start'
    #     }
    # }
    
    logger.info("SmithTeeTimeBooker_Start")
    
    twilioHandler = TwilioHandler(os.environ["Twilio_AccountSID"],
                                  os.environ["Twilio_AuthToken"],
                                  os.environ["Twilio_ToNumbers"],
                                  os.environ["Twilio_FromNumber"],
                                  logger)

    smithGolfHandler = SmithGolfHandler(os.environ["Smith_Url"],
                                        os.environ["Smith_PreferredTeeTimes"],
                                        os.environ["Smith_Username"],
                                        os.environ["Smith_Password"],
                                        twilioHandler,
                                        logger)

    smithGolfHandler.BookSmithTeeTimes()

    logger.info("SmithTeeTimeBooker ended.")
