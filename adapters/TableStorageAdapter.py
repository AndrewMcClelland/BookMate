from typing import List
from azure.cosmosdb.table.tableservice import TableService, TableBatch
from azure.cosmosdb.table.models import Entity

MAX_BATCH_SIZE = 100

class TableStorageAdapter:
    def __init__(self, accountName: str, accountKey: str) -> None:
        self.tableService = TableService(account_name=accountName, account_key=accountKey)
    
    def GetEntity(self, tableName: str, partitionKey: str, rowKey: str) -> Entity:
        if not partitionKey:
            raise ValueError("`partitionKey` is 'None' or empty.")
        if not rowKey:
            raise ValueError("`rowKey` is 'None' or empty.")
        
        return self.tableService.get_entity(tableName, partitionKey, rowKey)

    def GetEntities(self, tableName: str) -> List[Entity]:
        filter = ""
        
        return self.tableService.query_entities(tableName, filter)
    
    def GetEntities(self, tableName: str, partitionKey: str) -> List[Entity]:
        if not partitionKey:
            raise ValueError("`partitionKey` is 'None' or empty.")
        
        filter = "PartitionKey eq {0}".format(partitionKey)

        return self.tableService.query_entities(tableName, filter)

    def GetEntities(self, tableName: str, partitionKey: str, rowKeys: List[str]) -> List[Entity]:
        if not partitionKey:
            raise ValueError("`partitionKey` is 'None' or empty.")
        if not rowKeys:
            raise ValueError("`rowKeys` is 'None' or empty.")
        
        partitionKeyFilter = "PartitionKey eq {0}".format(partitionKey)
        rowKeysFilterList = ["RowKey eq {0}".format(rowKey) for rowKey in rowKeys]
        rowKeysFilter = " or ".join(rowKeysFilterList)

        filter = "{0} and ({1})".format(partitionKeyFilter, rowKeysFilter)

        return self.tableService.query_entities(tableName, filter)
    
    def InsertEntity(self, tableName: str, entity: Entity) -> None:
        if not entity:
            raise ValueError("`entity` is 'None' or empty.")
        
        self.tableService.insert_or_merge_entity(tableName, entity)

    def InsertEntities(self, tableName: str, entities: List[Entity]) -> None:
        if not entities:
            raise ValueError("`entities` is 'None' or empty.")
        
        batch = TableBatch()
        batchCount = 0

        for entity in entities:
            batch.insert_or_merge_entity(entity)
            batchCount += 1

            if batchCount > MAX_BATCH_SIZE:
                self.tableService.commit_batch(tableName, batch)
                batchCount = 0
                batch = TableBatch()
        
        if batchCount > 0:
            self.tableService.commit_batch(tableName, batch)