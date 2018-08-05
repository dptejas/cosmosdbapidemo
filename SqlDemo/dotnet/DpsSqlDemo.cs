using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DpsDemo
{
    internal sealed class BonsaiTree
    {
        public string id;
        public string name;
        public string height;
        public string age;
        public string price;
        public string scientificName;
    }
    class DpsSqlDemo
    {
        public static async Task CreateDoc(DocumentClient client)
        {
           var collectionLink = UriFactory.CreateDocumentCollectionUri(DpsResourceLinks._databaseId, DpsResourceLinks._collectionBonsaiTree);
           var trees = new List<object>();
            trees.Add(new BonsaiTree{ name = "Papaya Tree", id="papaya", height="1ft", price="1000Rs", age="5yrs"});
            trees.Add(new BonsaiTree{ name = "Mango Tree", id="mango", height="2ft", price="4000Rs", age="10yrs"});
            foreach(var tree in trees)
            {
                Document created = await client.CreateDocumentAsync(collectionLink, tree);
                Console.WriteLine("Created document");
            }
        }
        public static async Task ReadAllTrees(DocumentClient client)
        {
            Uri collectionLink = UriFactory.CreateDocumentCollectionUri(DpsResourceLinks._databaseId, DpsResourceLinks._collectionBonsaiTree);
            var docs = await client.ReadDocumentFeedAsync(collectionLink, new FeedOptions { MaxItemCount = 10 });
            Console.WriteLine("ReadAllTrees-------------");
            foreach (var d in docs)
            {
                Console.WriteLine(d);
            }
        }
        public static async Task ReadATree(DocumentClient client)
        {
            var response = await client.ReadDocumentAsync(
                UriFactory.CreateDocumentUri(DpsResourceLinks._databaseId, DpsResourceLinks._collectionBonsaiTree, "papaya"));
            Console.WriteLine("ReadATree-------------");
            Console.WriteLine("Tree by Id {0}", response.Resource);
        }
        public static async Task UpsertATree(DocumentClient client)
        {
            //Read document
            var collectionLink = UriFactory.CreateDocumentCollectionUri(DpsResourceLinks._databaseId, DpsResourceLinks._collectionBonsaiTree);
            Console.WriteLine("Upsert-------------");
            //Update height
            var updated = new BonsaiTree{ name = "Papaya Tree", id="papaya", height="2ft", price="1000Rs", age="5yrs"};
            var response1 = await client.UpsertDocumentAsync(collectionLink, updated);
        }
        public static async Task DeleteTree(DocumentClient client)
        {
            Console.WriteLine("Delete-------------");
            // Delete papaya 
            var response = await client.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(DpsResourceLinks._databaseId, DpsResourceLinks._collectionBonsaiTree, "papaya"));
            Console.WriteLine("Deleted");

            // Delete mango
            response = await client.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(DpsResourceLinks._databaseId, DpsResourceLinks._collectionBonsaiTree, "mango"));
            Console.WriteLine("Deleted");
        }
        public static async Task LinqQuery(DocumentClient client)
        {
            Uri collectionLink = UriFactory.CreateDocumentCollectionUri(DpsResourceLinks._databaseId, DpsResourceLinks._collectionBonsaiTree);
            Console.WriteLine("LinqQuery------------");
            // Simple Select query using parameters
            var bonsaiTree = from b in client.CreateDocumentQuery<BonsaiTree>(collectionLink)
                select b;                
            foreach(var b in bonsaiTree)
            {
                Console.WriteLine(b.name);
            }

            // Parameterized Select Query
            var query = client.CreateDocumentQuery<BonsaiTree>(collectionLink, new SqlQuerySpec(){
                QueryText = "Select * from BonsaiTreeTable b where b.id = @id",
                Parameters = new SqlParameterCollection()
                {
                    new SqlParameter("@id", "LemonTree")
                }    
            });
            Console.WriteLine("Found "+ query.ToList().Count()+" Trees");

            // Query with LINQ
            var q2 = client.CreateDocumentQuery<BonsaiTree>(collectionLink, new FeedOptions{MaxItemCount = 10})
                .Where(d => d.scientificName.Contains("Ci"));

            Console.WriteLine(q2.ToList().Count());
        }
    }
}
