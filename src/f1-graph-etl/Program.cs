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
        /// <remarks>
        /// Delaying the calls on purpose because of 'Responsible Use': http://ergast.com/mrd/terms. However,
        /// this model is really not working out well. So, I need to pull them in bulk.
        /// </remarks>
        public async Task Main(string[] args)
        {
            using (var client = new HttpClient() { BaseAddress = new Uri("http://ergast.com/") })
            {
                var seasons = await GetSeasonsAsync(client);
                foreach(var season in seasons)
                {
                    await Task.Delay(1000);
                    Console.WriteLine($"Year:{season.Year}");
                    var races = await GetRacesAsync(client, season.Year);
                    foreach(var race in races)
                    {
                        await Task.Delay(1000);
                        Console.WriteLine($"Round:{race.Round}, Name:{race.Name}, {race.Circuit.Location.Country}");
                    }
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

        private async Task<Race[]> GetRacesAsync(HttpClient client, int seasonYear)
        {
            // Well, limit=100 will work here for now as I know there are only 20ish races per season.
            var response = await client.GetAsync($"api/f1/{seasonYear}.json?limit=100");
            var content = await response.Content.ReadAsAsync<JObject>();
            var races = content["MRData"]["RaceTable"]["Races"].ToObject<IEnumerable<Race>>().ToArray();

            return races;
        }
    }

    public class Season
    {
        [JsonProperty(PropertyName = "season")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }

    public class Race
    {
        [JsonProperty(PropertyName = "season")]
        public int SeasonYear { get; set; }

        [JsonProperty(PropertyName = "round")]
        public byte Round { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "raceName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Circuit")]
        public Circuit Circuit { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }
    }

    public class Circuit
    {
        [JsonProperty(PropertyName = "circuitId")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "circuitName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Location")]
        public Location Location { get; set; }
    }

    public class Location
    {
        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "long")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "locality")]
        public string Locale { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
    }
}
