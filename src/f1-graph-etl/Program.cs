using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace F1Graph.Etl
{
    public class Program
    {
        public Program()
        {    
        }
        
        public async Task Main(string[] args)
        {
            using (var client = new HttpClient() { BaseAddress = new Uri("http://ergast.com/") })
            {
                var seasons = await GetSeasonsAsync(client);      
                foreach(var season in seasons)
                {
                    Console.WriteLine(season.Year);
                }
            }
        }
        
        private async Task<Season[]> GetSeasonsAsync(HttpClient client)
        {
            var response = await client.GetAsync("api/f1/seasons.json?limit=100");
            var content = await response.Content.ReadAsAsync<JObject>();
            var seasons = content["MRData"]["SeasonTable"]["Seasons"].ToObject<IEnumerable<Season>>().ToArray();
                
            return seasons;    
        }
    }
    
    public class Season 
    {
        [JsonProperty(PropertyName = "season")]
        public int Year { get; set; }
        
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}