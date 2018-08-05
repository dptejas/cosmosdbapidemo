
// Student -> Name, Age, Section, Class(PK), (ID)RK, Address
// Import module
// https://docs.microsoft.com/en-us/azure/cosmos-db/create-table-nodejs
var azure = require('azure-storage');
var entityGen;
var storageClient;
var tableName = 'Student';

// Create connection
var tableSvc = azure.createTableService(
	'kadurgadpsazuretable',
	'',//Key
	 'https://kadurgadpsazuretable.table.cosmosdb.azure.com:443/');

// Gets entity with parition key 4 and row key 11
tableSvc.retrieveEntity(tableName, '4', '11', function(error, result, response){
	if(!error){
		// result contains the entity
		console.log(result.Name);
	}
	else
	{
		  console.log(error);
	}
});