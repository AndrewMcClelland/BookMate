from typing import List
from azure.cosmosdb.table.tableservice import TableService, TableBatch
from azure.cosmosdb.table.models import Entity

MAX_BATCH_SIZE = 100

class TableStorageAdapter:
    def __init__(self, account_name: str, account_key: str) -> None:
        self.table_service = TableService(account_name=account_name, account_key=account_key)

    def get_entity(self, table_name: str, partition_key: str, row_key: str) -> Entity:
        if not partition_key:
            raise ValueError("`partition_key` is 'None' or empty.")
        if not row_key:
            raise ValueError("`row_key` is 'None' or empty.")

        return self.table_service.get_entity(table_name, partition_key, row_key)

    def get_all_entities(self, table_name: str, custom_filter_query: str = None) -> List[Entity]:

        return self.table_service.query_entities(table_name=table_name, filter=custom_filter_query)

    def get_all_partition_entities(self, table_name: str, partition_key: str, custom_filter_query: str = None) -> List[Entity]:
        if not partition_key:
            raise ValueError("`partition_key` is 'None' or empty.")

        filter_query = self.__combine_filter_queries__("PartitionKey eq {0}".format(partition_key), custom_filter_query)

        return self.table_service.query_entities(table_name=table_name, filter=filter_query)

    def get_entities(self, table_name: str, partition_key: str, row_keys: List[str], custom_filter_query: str = None) -> List[Entity]:
        if not partition_key:
            raise ValueError("`partition_key` is 'None' or empty.")
        if not row_keys:
            raise ValueError("`rowKeys` is 'None' or empty.")

        partition_key_filter = "PartitionKey eq {0}".format(partition_key)
        row_keys_filter_list = ["RowKey eq {0}".format(row_key) for row_key in row_keys]
        row_keys_filter = " or ".join(row_keys_filter_list)

        filter_query = self.__combine_filter_queries__("{0} and ({1})".format(partition_key_filter, row_keys_filter), custom_filter_query)

        return self.table_service.query_entities(table_name=table_name, filter=filter_query)

    def insert_entity(self, table_name: str, entity: Entity) -> None:
        if not entity:
            raise ValueError("`entity` is 'None' or empty.")

        self.table_service.insert_or_merge_entity(table_name, entity)

    def insert_entities(self, table_name: str, entities: List[Entity]) -> None:
        if not entities:
            raise ValueError("`entities` is 'None' or empty.")

        batch = TableBatch()
        batch_count = 0

        for entity in entities:
            batch.insert_or_merge_entity(entity)
            batch_count += 1

            if batch_count > MAX_BATCH_SIZE:
                self.table_service.commit_batch(table_name, batch)
                batch_count = 0
                batch = TableBatch()

        if batch_count > 0:
            self.table_service.commit_batch(table_name, batch)

    def delete_entity(self, table_name: str, entity: Entity) -> None:
        if not entity:
            raise ValueError("`entity` is 'None' or empty.")

        self.table_service.delete_entity(table_name, entity.PartitionKey, entity.RowKey)

    def delete_entities(self, table_name: str, entities: List[Entity]) -> None:
        if not entities:
            raise ValueError("`entities` is 'None' or empty.")

        batch = TableBatch()
        batch_count = 0

        for entity in entities:
            batch.delete_entity(entity.PartitionKey, entity.RowKey)
            batch_count += 1

            if batch_count > MAX_BATCH_SIZE:
                self.table_service.commit_batch(table_name, batch)
                batch_count = 0
                batch = TableBatch()

        if batch_count > 0:
            self.table_service.commit_batch(table_name, batch)

    def __combine_filter_queries__(self, *filter_queries: str) -> str:
        result = ""

        for filter_query in filter_queries:
            if filter_query and filter_query != "":
                new_query = "({0})".format(filter_query) if result == "" else " and {0}".format(new_query)
                result += new_query

        return result
