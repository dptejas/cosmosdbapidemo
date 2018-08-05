using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DpsDemo
{
    class DpsResourceLinks
    {
        public static readonly Uri _endpointUri = new Uri("https://kadurgadpssql.documents.azure.com:443/");
        public static readonly string _primaryKey = "";
        public static readonly string _databaseId = "MyBonsaiStoreDB";
        public static readonly string _collectionBonsaiTree = "BonsaiTreeTable";  
        public static readonly string _collectionBonsaiOrders = "BonsaiOrdersTable";                 
    }
}

