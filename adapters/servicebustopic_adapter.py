import json
from typing import Dict
from datetime import datetime
from azure.servicebus import ServiceBusClient, ServiceBusMessage

class ServiceBusTopicAdapter:
    def __init__(self, connection_string: str, topic_name: str) -> None:
        self.service_bus_client = ServiceBusClient.from_connection_string(conn_str=connection_string, logging_enable=True)
        self.topic_sender = self.service_bus_client.get_topic_sender(topic_name=topic_name)

    def send_message(self, object_to_send, custom_properties: Dict[str, str] = None, scheduled_enqueue_time_utc: datetime = None) -> None:
        message = ServiceBusMessage(body=self.__convert_to_message__(object_to_send),
                                    application_properties=custom_properties,
                                    scheduled_enqueue_time_utc=scheduled_enqueue_time_utc,
                                    content_type='application/json')

        self.topic_sender.send_messages(message)

    def __convert_to_message__(self, item) -> str:
        item_json = json.dumps(item.__dict__)
        return item_json.encode('utf8')
