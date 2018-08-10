using System;
using System.IO;
using Gremlin.Net;
using Gremlin.Net.Driver;
using Newtonsoft.Json;
using Gremlin.Net.Structure.IO.GraphSON;

namespace DpsGremlinSession
{
    class SampleQueries
    {
        public static void GetAllMoviesDirectedBy(GremlinClient gc, string name)
        {
            Console.WriteLine("GetAllMoviesDirectedBy");
            string query = String.Format("g.V().haslabel('person').has('id', '{0}').outE().haslabel('{1}').inV()", name, Relations.Directed);
            Utils.RunQuery(gc, query);
        }
        public static void GetAllActorsInMovie(GremlinClient gc, string title)
        {
            Console.WriteLine("GetAllActorsInMovie");
            string query = String.Format("g.V().haslabel('{0}').has('id', '{1}').inE().haslabel('{2}').outV()", Movie.Type,title, Relations.Acted);
            Utils.RunQuery(gc, query);
        }
        public static void GetAllMoviesActedBy(GremlinClient gc, string name)
        {
            Console.WriteLine("GetAllMoviesActedBy");
            string query = String.Format("g.V().haslabel('person').has('id', '{0}').outE().haslabel('{1}').inV()", name, Relations.Acted);
            Utils.RunQuery(gc, query);
            
        }
        public static void GetAllArtistsInMovie(GremlinClient gc, string name)
        {
            Console.WriteLine("GetAllMoviesActedBy");
            string query = String.Format("g.V().haslabel('movie').has('id','{0}').inE().outV()", name);
            Utils.RunQuery(gc, query);
            
        }
    }
}