
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

console.log('Table Service created');

// Get Top two entries with partition key as 4
var query = new azure.TableQuery()
  .top(2)
  .where('PartitionKey eq ?', '4');
  tableSvc.queryEntities(tableName,query, null, function(error, result, response) {
	  if(!error) {
		// query was successful
		console.log(result.entries);
	  }
	});