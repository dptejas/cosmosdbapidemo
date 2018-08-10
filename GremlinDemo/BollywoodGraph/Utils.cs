using System;
using System.IO;
using Gremlin.Net;
using Gremlin.Net.Driver;
using Newtonsoft.Json;
using Gremlin.Net.Structure.IO.GraphSON;

namespace DpsGremlinSession
{
    class Utils
    {
        public static void RunQuery(GremlinClient gc, string query)
        {
            var task = gc.SubmitAsync<dynamic>(query);
            task.Wait();
            Console.WriteLine("In Run Query "+query);
            foreach (var result in task.Result)
            {
                string output = JsonConvert.SerializeObject(result);
                Console.WriteLine(String.Format("\tResult:\n\t{0}", output));
            }
        }
    }
    class Configs
    {
         public static string CsvPath = "BollywoodMovieDetail.csv";
         public static string Hostname = "kadurgadpsgremlinsession.gremlin.cosmosdb.azure.com";
         public static int Port = 443;
         public static string AuthKey = "";
         public static string Database = "ConnectionDB";
         public static string Collection = "ProductRecommendation";
    }
   class Relations
   {
       public static string Acted = "acted";
       public static string Directed = "directed";
       public static string Wrote = "wrote";
       public static string Released = "released";
       public static string InGenre = "ingenre";
   }
    public class MovieCsv
    {
        public string imdbId;
        public string title;
        public string releaseYear;
        public string releaseDate;
        public string genre;
        public string writers;
        public string actors;
        public string directors;
        public string sequel;
        public string hitFlop;
    }
    public class Movie{
        public string title;
        public string imdbId;
        public static string Type = "movie";
    }
    public class Person{
        public string name;
        public string wikiLink;
        public static string Type = "person";
    }
    public class Director : Person{
    }
    public class Actor : Person{
    }
    public class Writer : Person{
    }
    
    public class Genre{
        public string name;
        public static string Type = "genre";
    }
    public class Year{
        public string name;
        public static string Type = "year";
    }
}
