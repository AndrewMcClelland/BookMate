import json
from typing import List, Dict
from datetime import datetime
from azure.servicebus import ServiceBusClient, ServiceBusMessage

class ServiceBusTopicAdapter:
    def __init__(self, connectionString: str, topicName: str) -> None:
        self.serviceBusClient = ServiceBusClient.from_connection_string(conn_str=connectionString, logging_enable=True)
        self.topicSender = self.serviceBusClient.get_topic_sender(topic_name=topicName)
    
    def SendMessage(self, objectToSend, customProperties: Dict[str, str] = None, scheduledEnqueueTimeUtc: datetime = None) -> None:
        message = ServiceBusMessage(body=self.__ConvertToMessage__(objectToSend),
                                    application_properties=customProperties,
                                    scheduled_enqueue_time_utc=scheduledEnqueueTimeUtc,
                                    content_type='application/json')
        
        self.topicSender.send_messages(message)
    
    def __ConvertToMessage__(self, item) -> str:
        itemJson = json.dumps(item.__dict__)
        return itemJson.encode('utf8')
