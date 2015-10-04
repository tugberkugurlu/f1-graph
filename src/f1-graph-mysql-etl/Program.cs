using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using F1.Domain;
using MongoDB.Driver;
using Nte.Identity.Domain;
using F1.Domain.Entities;
using F1.Domain.ValueObjects;

namespace F1Graph.MySql.Etl
{
    /// <remarks>
    /// <para> - The race date inside the 'races' table in MySQL database is UTC time on the specified date.</para>
    /// </remarks>
    public class Program
    {
        private readonly string _mySqlConnectionString = "Data Source=172.17.0.12;port=3306;Initial Catalog=f1;User Id=root;password=1234567890";
        private readonly string _mongoDbConnectionStr = "mongodb://172.17.0.16:27017";

        public async Task Main(string[] args)
        {    
            var mongoDatabase = ConfigureAndGetMongoDatabase();
            var driversCollection = mongoDatabase.GetCollection<DriverEntity>("drivers");
            var constructorsCollection = mongoDatabase.GetCollection<ConstructorEntity>("constructors");
            var circuitsCollection = mongoDatabase.GetCollection<CircuitEntity>("circuits");
            var seasonsCollection = mongoDatabase.GetCollection<SeasonEntity>("seasons");
            driversCollection.Drop();
            constructorsCollection.Drop();
            circuitsCollection.Drop();
            seasonsCollection.Drop();

            Console.WriteLine("trying to get the drivers now...");
            try
            {
                var drivers = GetDrivers().Select(driver => driver.ToEntity());
                await driversCollection.InsertManyAsync(drivers);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }

            Console.WriteLine("trying to get the constructors now...");
            try
            {
                var constructors = GetConstructors().Select(ctor => ctor.ToEntity());
                await constructorsCollection.InsertManyAsync(constructors);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }

            Console.WriteLine("trying to get the circuits now...");
            try
            {
                var circuits = GetCircuits().Select(circuit => circuit.ToEntity());
                await circuitsCollection.InsertManyAsync(circuits);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }

            Console.WriteLine("trying to get the seasons now...");
            try
            {
                var seasons = GetSeasons().Select(season => season.ToEntity());
                await seasonsCollection.InsertManyAsync(seasons);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }

        private IMongoDatabase ConfigureAndGetMongoDatabase()
        {
            var client = new MongoClient(_mongoDbConnectionStr);
            var database = client.GetDatabase("f1");

            MongoConfig.Configure(MongoConvetionsConfig.RegisterGlobalConventions);

            return database;
        }

        private IEnumerable<Driver> GetDrivers()
        {
            return Get("SELECT * FROM drivers", r =>
            {
                var driver = new Driver
                {
                    Id = int.Parse(r["driverId"].ToString()),
                    RefCode = r["driverRef"].ToString(),
                    Firstname = r["forename"].ToString(),
                    Surname = r["surname"].ToString(),
                    Nationality = r["nationality"].ToString(),
                    Url = r["url"].ToString()
                };

                DateTime dob;
                string dobStr = r["dob"].ToString();
                if (DateTime.TryParse(dobStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out dob) == false)
                {
                    Console.WriteLine("Couldn't parse the dob: " + dobStr + "; DriverId: " + driver.Id);
                }
                else
                {
                    driver.DateOfBirth = dob;
                }

                return driver;
            });
        }

        private IEnumerable<Constructor> GetConstructors()
        {
            return Get("SELECT * FROM constructors", r => new Constructor
            {
                Id = int.Parse(r["constructorId"].ToString()),
                ConstructorRef = r["constructorRef"].ToString(),
                Name = r["name"].ToString(),
                Nationality = r["nationality"].ToString(),
                Url = r["url"].ToString()
            });
        }

        private IEnumerable<Circuit> GetCircuits()
        {
            return Get("SELECT * FROM circuits", r => new Circuit
            {
                Id = int.Parse(r["circuitId"].ToString()),
                CircuitRef = r["circuitRef"].ToString(),
                Name = r["name"].ToString(),
                Location = r["location"].ToString(),
                Country = r["country"].ToString(),
                Lat = float.Parse(r["lat"].ToString()),
                Lng = float.Parse(r["lng"].ToString()),
                Url = r["url"].ToString()
            });
        }

        private IEnumerable<Season> GetSeasons()
        {
            return Get("select * from seasons order by year", r => new Season
            {
                Year = int.Parse(r["year"].ToString()),
                Url = r["url"].ToString()
            });
        }

        private IEnumerable<TModel> Get<TModel>(string query, Func<DbDataReader, TModel> projection)
        {
            using (DbConnection connection = new MySqlConnection(_mySqlConnectionString))
            using (DbCommand cmd = new MySqlCommand(query))
            {
                connection.Open();
                cmd.Connection = connection;
                var reader = cmd.ExecuteReader();

                return reader.Select(projection).ToList();
            }
        }
    }

    public class Circuit
    {
        public int Id { get; set; }
        public string CircuitRef { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public string Url { get; set; }
    }

    public class Constructor
    {
        public int Id { get; set; }
        public string ConstructorRef { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string Url { get; set; }
    }

    public class Driver
    {
        public int Id { get; set; }
        public string RefCode { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string Url { get; set; }
    }

    public class Season
    {
        public int Year { get; set; }
        public string Url { get; set; }
    }

    public static class Extensions
    {
        public static IEnumerable<T> Select<T>(this DbDataReader reader, Func<DbDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }

        public static DriverEntity ToEntity(this Driver driver)
        {
            return driver.DateOfBirth != null
                ? new DriverEntity(driver.Id.ToString(CultureInfo.InvariantCulture), driver.Firstname, driver.Surname, driver.Nationality, driver.Url, driver.DateOfBirth.Value)
                : new DriverEntity(driver.Id.ToString(CultureInfo.InvariantCulture), driver.Firstname, driver.Surname, driver.Nationality, driver.Url);
        }

        public static ConstructorEntity ToEntity(this Constructor constructor)
        {
            return new ConstructorEntity(
                constructor.Id.ToString(CultureInfo.InvariantCulture),
                constructor.Name,
                constructor.Nationality,
                constructor.Url);
        }

        public static CircuitEntity ToEntity(this Circuit circuit)
        {
            var location = new Location(circuit.Lat, circuit.Lng);

            return new CircuitEntity(
                circuit.Id.ToString(CultureInfo.InvariantCulture),
                circuit.Name,
                circuit.Location,
                circuit.Country,
                location,
                circuit.Url);
        }

        public static SeasonEntity ToEntity(this Season season)
        {
            return new SeasonEntity(season.Year, season.Url);
        }

        public static void Drop<TEntity>(this IMongoCollection<TEntity> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            collection.Database.DropCollectionAsync(collection.CollectionNamespace.CollectionName).Wait();
        }
    }
}
