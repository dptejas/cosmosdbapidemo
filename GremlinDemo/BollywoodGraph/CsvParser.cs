using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Gremlin.Net;
using Gremlin.Net.Driver;
using Newtonsoft.Json;
using Gremlin.Net.Structure.IO.GraphSON;

namespace DpsGremlinSession
{
    class CsvParser
    {
        public static MovieCsv[] ParseFile(string fileName)
        {
            List<MovieCsv> returnList = new List<MovieCsv>();
            StreamReader file = new StreamReader( fileName );
            string line;
            int count = 0;
            while ((line = file.ReadLine()) != null) {
                if(count == 0)
                {
                    count++;
                    continue;
                }
                else
                {
                    string[] arr = line.Split(",");
                    MovieCsv mc = new MovieCsv();
                    mc.imdbId = arr[0];
                    mc.title = arr[1];
                    mc.releaseYear = arr[2];
                    mc.releaseDate = arr[3];
                    mc.genre = arr[4];
                    mc.writers = arr[5];
                    mc.actors = arr[6];
                    mc.directors = arr[7];
                    mc.sequel = arr[8];
                    mc.hitFlop = arr[9];
                    returnList.Add(mc);
                    count++;
                }                
            }
            return returnList.ToArray();
        }
    }
}
