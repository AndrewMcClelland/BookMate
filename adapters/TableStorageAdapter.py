from typing import TypeVar, Generic, List, Dict
from azure.cosmosdb.table.tableservice import TableService, TableBatch
from azure.cosmosdb.table.models import Entity

T = TypeVar('T')
MAX_BATCH_SIZE = 100

class TableStorageAdapter(Generic[T]):
    def __init__(self, accountName: str, accountKey: str) -> None:
        self.tableService = TableService(account_name=accountName, account_key=accountKey)
        self.tableName = type(T).__name__
    
    def GetEntity(self, partitionKey: str, rowKey: str) -> T:
        if not partitionKey:
            raise ValueError("`partitionKey` is 'None' or empty.")
        if not rowKey:
            raise ValueError("`rowKey` is 'None' or empty.")
        
        return self.tableService.get_entity(self.tableName, partitionKey, rowKey)

    def GetEntities(self) -> List[T]:
        filter = ""
        
        return self.tableService.query_entities(self.tableName, filter)
    
    def GetEntities(self, partitionKey: str) -> List[T]:
        if not partitionKey:
            raise ValueError("`partitionKey` is 'None' or empty.")
        
        filter = "PartitionKey eq {0}".format(partitionKey)

        return self.tableService.query_entities(self.tableName, filter)

    def GetEntities(self, partitionKey: str, rowKeys: List[str]) -> List[T]:
        if not partitionKey:
            raise ValueError("`partitionKey` is 'None' or empty.")
        if not rowKeys:
            raise ValueError("`rowKeys` is 'None' or empty.")
        
        partitionKeyFilter = "PartitionKey eq {0}".format(partitionKey)
        rowKeysFilterList = ["RowKey eq {0}".format(rowKey) for rowKey in rowKeys]
        rowKeysFilter = " or ".join(rowKeysFilterList)

        filter = "{0} and ({1})".format(partitionKeyFilter, rowKeysFilter)

        return self.tableService.query_entities(self.tableName, filter)
    
    def InsertEntity(self, entity) -> None:
        if not entity:
            raise ValueError("`entity` is 'None' or empty.")
        
        self.tableService.insert_or_merge_entity(self.tableName, entity)

    def InsertEntities(self, entities) -> None:
        if not entities:
            raise ValueError("`entities` is 'None' or empty.")
        
        batch = TableBatch()
        batchCount = 0

        for entity in entities:
            batch.insert_or_merge_entity(entity)
            batchCount += 1

            if batchCount > MAX_BATCH_SIZE:
                self.tableService.commit_batch(self.tableName, batch)
                batchCount = 0
                batch = TableBatch()
        
        if batchCount > 0:
            self.tableService.commit_batch(self.tableName, batch)