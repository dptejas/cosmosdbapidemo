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
            ExecuteRestDemo();
        }
        
        public static void ExecuteRestDemo()
        {    
            // REST Demo
            DpsRestDemo.ListAllDbs();
            DpsRestDemo.GetDatabase();
            DpsRestDemo.GetAllCollections();
            DpsRestDemo.GetAllDocuments();
            DpsRestDemo.GetADocument();
        }
    }
}
