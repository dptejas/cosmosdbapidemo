using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;

namespace DpsDemo
{
    class DpsRestDemo
    {
        public static void ListAllDbs()
        {
            // Initialize Client
            string utc_date = DateTime.UtcNow.ToString("r");
            var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Add("x-ms-date", utc_date);
            client.DefaultRequestHeaders.Add("x-ms-version", "2015-08-06");
            
            // Process GET All Dbs
            // Sample url https://kadurgadpssql.documents.azure.com:443/dbs
            string verb = "GET";
            string resourceId = "";
            string resourceType = "dbs";
            string masterKey = DpsResourceLinks._primaryKey;
            string authHeader = GenerateMasterKeyAuthorizationSignature(verb, resourceId, resourceType, masterKey, "master", "1.0");
            string resourceLink = string.Format("dbs");

            client.DefaultRequestHeaders.Remove("authorization");
            client.DefaultRequestHeaders.Add("authorization", authHeader);
            var response = client.GetStringAsync(new Uri(DpsResourceLinks._endpointUri, resourceLink)).Result;
            Console.WriteLine("All DBs");
            Console.WriteLine(response);
        }
        public static void GetDatabase()
        {
            // Create HTTP Client
            string utc_date = DateTime.UtcNow.ToString("r");
            var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Add("x-ms-date", utc_date);
            client.DefaultRequestHeaders.Add("x-ms-version", "2015-08-06");
            
            // GET a database
            // Sample url https://kadurgadpssql.documents.azure.com:443/dbs/MyBonsaiStoreDB/
            string verb = "GET";
            string resourceType = "dbs";
            string resourceLink = "dbs/" + DpsResourceLinks._databaseId;
            string resourceId = resourceLink;
            string authHeader = GenerateMasterKeyAuthorizationSignature(verb, resourceId, resourceType, DpsResourceLinks._primaryKey, "master", "1.0");

            client.DefaultRequestHeaders.Remove("authorization");
            client.DefaultRequestHeaders.Add("authorization", authHeader);
            var response = client.GetStringAsync(new Uri(DpsResourceLinks._endpointUri, resourceLink)).Result;
            Console.WriteLine("Get a Database");
            Console.WriteLine(response);
        }
        public static void GetAllCollections()
        {
            // Create HTTP Client
            string utc_date = DateTime.UtcNow.ToString("r");
            var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Add("x-ms-date", utc_date);
            client.DefaultRequestHeaders.Add("x-ms-version", "2015-08-06");
            
            // Sample url https://kadurgadpssql.documents.azure.com:443/dbs/MyBonsaiStoreDB/colls/
            string verb = "GET";
            string resourceType = "colls";
            string resourceLink = string.Format("dbs/{0}/colls", DpsResourceLinks._databaseId);
            string resourceId = string.Format("dbs/{0}", DpsResourceLinks._databaseId) ;
            string authHeader = GenerateMasterKeyAuthorizationSignature(
                verb, resourceId, resourceType, DpsResourceLinks._primaryKey, "master", "1.0");
            
            client.DefaultRequestHeaders.Remove("authorization");
            client.DefaultRequestHeaders.Add("authorization", authHeader);
            
            // Call api
            var response = client.GetStringAsync(new Uri(DpsResourceLinks._endpointUri, resourceLink)).Result;
            Console.WriteLine("Get collections");
            Console.WriteLine(response);
        }
        public static void GetAllDocuments()
        {
            // Create HTTP Client
            string utc_date = DateTime.UtcNow.ToString("r");
            var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Add("x-ms-date", utc_date);
            client.DefaultRequestHeaders.Add("x-ms-version", "2015-08-06");
            
            // Sample url https://kadurgadpssql.documents.azure.com:443/dbs/MyBonsaiStoreDB/colls/BonsaiTreeTable/docs
            string verb = "GET";
            string resourceType = "docs";
            string resourceLink = string.Format("dbs/{0}/colls/{1}/docs", DpsResourceLinks._databaseId, DpsResourceLinks._collectionBonsaiTree);
            string resourceId = string.Format("dbs/{0}/colls/{1}", 
                DpsResourceLinks._databaseId, DpsResourceLinks._collectionBonsaiTree);
            string authHeader = GenerateMasterKeyAuthorizationSignature(
                verb, resourceId, resourceType, DpsResourceLinks._primaryKey, "master", "1.0");
            
            client.DefaultRequestHeaders.Remove("authorization");
            client.DefaultRequestHeaders.Add("authorization", authHeader);
            
            // Call api
            var response = client.GetStringAsync(new Uri(DpsResourceLinks._endpointUri, resourceLink)).Result;
            Console.WriteLine("Get all documents");
            Console.WriteLine(response);
        }
        public static void GetADocument()
        {
            // Create HTTP Client
            string utc_date = DateTime.UtcNow.ToString("r");
            var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Add("x-ms-date", utc_date);
            client.DefaultRequestHeaders.Add("x-ms-version", "2015-08-06");
            
            // Sample url https://kadurgadpssql.documents.azure.com:443/dbs/MyBonsaiStoreDB/colls/BonsaiTreeTable/docs/peepaltree
            string verb = "GET";
            string resourceType = "docs";
            string resourceLink = string.Format("dbs/{0}/colls/{1}/docs/{2}", 
                DpsResourceLinks._databaseId, DpsResourceLinks._collectionBonsaiTree, "PeepalTree");
            string resourceId =resourceLink;
            string authHeader = GenerateMasterKeyAuthorizationSignature(
                verb, resourceId, resourceType, DpsResourceLinks._primaryKey, "master", "1.0");
            
            client.DefaultRequestHeaders.Remove("authorization");
            client.DefaultRequestHeaders.Add("authorization", authHeader);
            
            // Call api
            var response = client.GetStringAsync(new Uri(DpsResourceLinks._endpointUri, resourceLink)).Result;
            Console.WriteLine("Get a document");
            Console.WriteLine(response);
        }
        
        private static string GenerateMasterKeyAuthorizationSignature(string verb, string resourceId, string resourceType, string key, string keyType, string tokenVersion)
        {
            string utc_date = DateTime.UtcNow.ToString("r");
            var hmacSha256 = new System.Security.Cryptography.HMACSHA256 { Key = Convert.FromBase64String(key) };
            string payLoad = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}\n{1}\n{2}\n{3}\n{4}\n",
                    verb.ToLowerInvariant(),
                    resourceType.ToLowerInvariant(),
                    resourceId,
                    utc_date.ToLowerInvariant(),
                    ""
            );

            byte[] hashPayLoad = hmacSha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(payLoad));
            string signature = Convert.ToBase64String(hashPayLoad);
            return System.Web.HttpUtility.UrlEncode(String.Format(System.Globalization.CultureInfo.InvariantCulture, "type={0}&ver={1}&sig={2}",
                keyType,
                tokenVersion,
                signature));

        }
    }
}
