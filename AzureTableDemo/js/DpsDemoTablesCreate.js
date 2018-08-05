
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

//1. Create a Table and insert some rows
tableSvc.createTableIfNotExists(tableName, function(error, result, response){
  if(!error){
		// Table exists or created
		console.log('Table Created or exists!!');
		var student1 = {PartitionKey: {'_':'4'},  RowKey: {'_':'9'}, Name: {'_':'nobita'}, Age: {'_':'10'},  Allergies: {'_':'water'}, EmergencyContact: {'_':'nobita dad'}};
		addEntry(student1);
		
		var student2 = {PartitionKey: {'_':'4'},  RowKey: {'_':'10'}, Name: {'_':'shizuka'}, Age: {'_':'9'},  Allergies: {'_':'dust'},  Allergies: {'_':'dust'}, EmergencyContact: {'_':'dad'}};
		addEntry(student2);
		
		var student3 = {PartitionKey: {'_':'4'},  RowKey: {'_':'11'}, Name: {'_':'suneo'}, Age: {'_':'9.5'}, EmergencyContact: {'_':'suneo dad'}};
		addEntry(student3);
		
		var student4 = {PartitionKey: {'_':'3'},  RowKey: {'_':'12'}, Name: {'_':'jian'}, Age: {'_':'9.5'}, EmergencyContact: {'_':'jian mom'}};
		addEntry(student4);
	}
});

// Add Entry to Student table
function addEntry(student)
{
	tableSvc.insertEntity(tableName, student, function (error, result, response) {
		if(!error){
			console.log("Record Inserted");
		}
	});
}