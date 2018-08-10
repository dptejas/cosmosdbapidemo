using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gremlin.Net;
using Gremlin.Net.Driver;
using Newtonsoft.Json;
using Gremlin.Net.Structure.IO.GraphSON;

namespace DpsGremlinSession
{
    class DpsGremlinSession
    {
        static void Main(string[] args)
        {
            //Parse the csv file
            MovieCsv[] mc = CsvParser.ParseFile(Configs.CsvPath);
            
            var gremlinServer = new GremlinServer(Configs.Hostname, Configs.Port, enableSsl: true, 
                username: "/dbs/" + Configs.Database + "/colls/" + Configs.Collection, 
                password: Configs.AuthKey);

            using (var gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), 
                GremlinClient.GraphSON2MimeType))
            {
                //CreateActors(gremlinClient, "Laxmikant Berde");
                //DropAll(gremlinClient);
                //AddNodes(gremlinClient, mc); 
                //GetAll(gremlinClient);
                SampleQueries.GetAllMoviesDirectedBy(gremlinClient, "Sanjay Leela Bhansali");
                SampleQueries.GetAllActorsInMovie(gremlinClient, "Kabhi Khushi Kabhie Gham...");
                SampleQueries.GetAllMoviesActedBy(gremlinClient, "Aishwarya Rai Bachchan");
                SampleQueries.GetAllArtistsInMovie(gremlinClient, "The Legend of Bhagat Singh");
            }
        }
        public static void GetAll(GremlinClient gc)
        {
            string query = "g.V()";
            var task = gc.SubmitAsync<dynamic>(query);
            task.Wait();
            foreach (var result in task.Result)
            {
                string output = JsonConvert.SerializeObject(result);
                Console.WriteLine(String.Format("\tResult:\n\t{0}", output));
            }
        }
        public static void DropAll(GremlinClient gremlinClient)
        {
            Console.WriteLine("Dropping all edges and vertices");
            string cleanup = "g.E().drop()";
            var task = gremlinClient.SubmitAsync<dynamic>(cleanup);
            task.Wait();

            cleanup = "g.V().drop()";
            task = gremlinClient.SubmitAsync<dynamic>(cleanup);
            task.Wait();
        }
        public static bool CreateMovie(GremlinClient gc, string title, string wikiLink)
        {
            Console.WriteLine(title);
            if(!EntityExists(gc, title, Movie.Type))
            {
                string query = "g.addV('"+Movie.Type+
                "').property('id', '"+title+
                "').property('wikiLink', '"+wikiLink+"')";
                var task = gc.SubmitAsync<dynamic>(query);
                task.Wait();
                foreach (var result in task.Result)
                {
                    // The vertex results are formed as Dictionaries with a nested dictionary for their properties
                    string output = JsonConvert.SerializeObject(result);
                    Console.WriteLine(String.Format("\tResult:\n\t{0}", output));
                }
                return true;
            }
            else{
                return false;
            }
            
        }
        public static void GetWriterAndType(string name, ref string writer, ref string type)
        {
            if(name.IndexOf('(') >= 0)
            {
                writer = name.Substring(0,name.IndexOf('(')).Trim();
                type = name.Substring(name.IndexOf("(")+1, name.IndexOf(")")- name.IndexOf("(")-1).Trim();
            }
            else
            {
                writer = name.Trim();
                type = "";
            }
            Console.WriteLine(writer + "<->"+type+"<->");
        }
        public static void CreateGenre(GremlinClient gc, string genre)
        {
            string[] parsedGenre = genre.Split("|");
            foreach(var pa in parsedGenre)
            {
                //Get the genre, add if not exists
                string name = pa.Trim();
                string wikiLink = "";
                if(!EntityExists(gc, name, Genre.Type))
                {
                    string query = "g.addV('"+Genre.Type+
                        "').property('id', '"+name+
                        "').property('wikiLink', '"+wikiLink+"')";
                    var task = gc.SubmitAsync<dynamic>(query);
                    task.Wait();
                    foreach (var result in task.Result)
                    {
                        // The vertex results are formed as Dictionaries with a nested dictionary for their properties
                        string output = JsonConvert.SerializeObject(result);
                        Console.WriteLine(String.Format("\tResult:\n\t{0}", output));
                    }
                }
            } 
        }
        public static void CreateDirectors(GremlinClient gc, string directors)
        {
            string[] parsedDirectors = directors.Split("|");
            foreach(var pa in parsedDirectors)
            {
                //Get the director, add if not exists
                string name = pa.Trim();
                string wikiLink = "";
                if(name.Equals("N/A")) continue;
                if(!EntityExists(gc, name, Director.Type))
                {
                    string query = "g.addV('"+Director.Type+
                        "').property('id', '"+name+
                        "').property('wikiLink', '"+wikiLink+"')";
                    var task = gc.SubmitAsync<dynamic>(query);
                    task.Wait();
                    foreach (var result in task.Result)
                    {
                        // The vertex results are formed as Dictionaries with a nested dictionary for their properties
                        string output = JsonConvert.SerializeObject(result);
                        Console.WriteLine(String.Format("\tResult:\n\t{0}", output));
                    }
                }
            } 
        }
        public static void CreateActors(GremlinClient gc, string actors)
        {
            string[] parsedActors = actors.Split("|");
            foreach(var pa in parsedActors)
            {
                //Get the actor, add if not exists
                string name = pa.Trim();
                string wikiLink = "";
                Console.WriteLine("Inserting "+ name+" " + Actor.Type);
                if(name.Equals("N/A"))
                    continue;
                if(!EntityExists(gc, name, Actor.Type))
                {
                    string query = "g.addV('"+Actor.Type+
                        "').property('id', '"+name+
                        "').property('wikiLink', '"+wikiLink+"')";
                    Console.WriteLine(query);
                    var task = gc.SubmitAsync<dynamic>(query);
                    task.Wait();
                    foreach (var result in task.Result)
                    {
                        // The vertex results are formed as Dictionaries with a nested dictionary for their properties
                        string output = JsonConvert.SerializeObject(result);
                        Console.WriteLine(String.Format("\tResult:\n\t{0}", output));
                    }
                }
            }            
        }
        public static void CreateReleaseYear(GremlinClient gc, string year)
        {
            //Get the director, add if not exists
            if(!EntityExists(gc, year, Year.Type))
            {
                string query = "g.addV('"+Year.Type+
                    "').property('id', '"+year+"').";
                var task = gc.SubmitAsync<dynamic>(query);
                task.Wait();
                foreach (var result in task.Result)
                {
                    // The vertex results are formed as Dictionaries with a nested dictionary for their properties
                    string output = JsonConvert.SerializeObject(result);
                    Console.WriteLine(String.Format("\tResult:\n\t{0}", output));
                }
            }
        }
        public static void CreateWriters(GremlinClient gc, string writers)
        {
            string[] parsedWriters = writers.Split("|");
            foreach(var pa in parsedWriters)
            {
                //Get the actor, add if not exists
                string name = pa.Trim();
                string writer = "", category = "";
                GetWriterAndType(name,ref writer, ref category);
                if(writer.Equals("N/A")) continue;
                string wikiLink = "";
                if(!EntityExists(gc, writer, Writer.Type))
                {
                    string query = "g.addV('"+Writer.Type+
                        "').property('id', '"+writer+
                        "').property('wikiLink', '"+wikiLink+"')";
                    var task = gc.SubmitAsync<dynamic>(query);
                    task.Wait();
                    foreach (var result in task.Result)
                    {
                        // The vertex results are formed as Dictionaries with a nested dictionary for their properties
                        string output = JsonConvert.SerializeObject(result);
                        Console.WriteLine(String.Format("\tResult:\n\t{0}", output));
                    }
                }
            }            
        }
        public static bool EntityExists(GremlinClient gc, string name, string type)
        {
            string getQuery = "g.V().has('"+type+"', 'id', '"+name+"')";
            try
            {
                var task = gc.SubmitAsync<dynamic>(getQuery);
                task.Wait();
                if(task.Result.Count() == 0)
                {
                    // Create it
                    return false;
                }
                else
                {
                    Console.WriteLine("Already Exists");
                    return true;
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }
        public static void AssociateActorsWithMovie(GremlinClient gc, string title, string actors)
        {
            string[] listOfActors = actors.Split("|");
            string querySubPart = String.Format(".addE('{0}').to(g.V().haslabel('{1}').has('id','{2}'))", Relations.Acted, Movie.Type, title.Trim());

            foreach(var actor in listOfActors)
            {
                string query = String.Format("g.V().haslabel('{0}').has('id','{1}')", Actor.Type, actor.Trim())+querySubPart;
                Console.WriteLine(query);
                Utils.RunQuery(gc, query);
            }
        }
        public static void AssociateDirectorsWithMovie(GremlinClient gc, string title, string directors)
        {
            string[] listOfDirectors = directors.Split("|");
            string querySubPart = String.Format(".addE('{0}').to(g.V().haslabel('{1}').has('id','{2}'))", Relations.Directed, Movie.Type, title.Trim());

            foreach(var director in listOfDirectors)
            {
                string query = String.Format("g.V().haslabel('{0}').has('id','{1}')", Director.Type, director.Trim())+querySubPart;
                Console.WriteLine(query);
                Utils.RunQuery(gc, query);
            }
        }
        public static void AssociateWritersWithMovie(GremlinClient gc, string title, string writers)
        {
            string[] listOfWriters = writers.Split("|");
            string querySubPart = String.Format(".addE('{0}').to(g.V().haslabel('{1}').has('id','{2}'))", Relations.Wrote, Movie.Type, title.Trim());

            foreach(var name in listOfWriters)
            {
                string writer = "", category = "";
                GetWriterAndType(name,ref writer, ref category);
                string query = String.Format("g.V().haslabel('{0}').has('id','{1}')", Writer.Type, writer.Trim())+querySubPart;
                Console.WriteLine(query);
                Utils.RunQuery(gc, query);
            }
        }
        public static void AssociateGenresWithMovie(GremlinClient gc, string title, string genre)
        {
            string[] listOfGenres = genre.Split("|");
            
            foreach(var name in listOfGenres)
            {
                string query = String.Format("g.V().haslabel('{0}').has('id','{1}')", Movie.Type, title.Trim());
                string querySubPart = String.Format(".addE('{0}').to(g.V().haslabel('{1}').has('id','{2}'))", Relations.InGenre, Genre.Type, name.Trim());
                query = query+querySubPart;
                Console.WriteLine(query);
                Utils.RunQuery(gc, query);
            }
        }
        public static void AssociateReleaseYearWithMovie(GremlinClient gc, string title, string releaseYear)
        {
            string query = String.Format("g.V().haslabel('{0}').has('id','{1}')", Movie.Type, title.Trim());
            string querySubPart = String.Format(".addE('{0}').to(g.V().haslabel('{1}').has('id','{2}'))", Relations.Released, Year.Type, releaseYear.Trim());
            query = query+querySubPart;
            Console.WriteLine(query);
            Utils.RunQuery(gc, query);
        }
        
        public static void AddNodes(GremlinClient gremlinClient, MovieCsv[] mc)
        {
            foreach(var m in mc)
            {
                //Create movie node
                // Console.WriteLine("Creating Movies");
                // if(!CreateMovie(gremlinClient, m.title, ""))
                // {
                //     //Movie already created dont add actors and others
                //     continue;
                // }

                // //Create Actors
                // Console.WriteLine("Creating Actors");
                // CreateActors(gremlinClient, m.actors);

                // //Create Directors
                // Console.WriteLine("Creating Directors");
                // CreateDirectors(gremlinClient, m.directors);

                // //Create Genre
                // Console.WriteLine("Creating Genre");
                // CreateGenre(gremlinClient, m.genre);

                // //Create Years
                // Console.WriteLine("Creating Years");
                // CreateReleaseYear(gremlinClient, m.releaseYear);

                // //Create Writers
                // Console.WriteLine("Creating Writers");
                // CreateWriters(gremlinClient, m.writers);

                //Create Association with movie
                AssociateActorsWithMovie(gremlinClient, m.title, m.actors);
                AssociateDirectorsWithMovie(gremlinClient, m.title, m.directors);
                AssociateWritersWithMovie(gremlinClient, m.title, m.writers);
                AssociateGenresWithMovie(gremlinClient, m.title, m.genre);
                AssociateReleaseYearWithMovie(gremlinClient, m.title, m.releaseYear);
            }
        }
    }
}
