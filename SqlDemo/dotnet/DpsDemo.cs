using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DpsDemo
{
    class DpsDemo
    {
        public static async Task Main(string[] args)
        {
            await ExecuteSqlDemo();
        }
        public static async Task ExecuteSqlDemo()
        {
            using(DocumentClient client = new DocumentClient(DpsResourceLinks._endpointUri, DpsResourceLinks._primaryKey))
            {
                // Sql Document CRUD
                Console.WriteLine("Starting SqlDemo");
                //await DpsSqlDemo.CreateDoc(client);
                 await DpsSqlDemo.ReadAllTrees(client);
                 await DpsSqlDemo.ReadATree(client);
                // await DpsSqlDemo.UpsertATree(client);
                //await DpsSqlDemo.LinqQuery(client);
                //await DpsSqlDemo.DeleteTree(client);                
            }
        }
    }
}
