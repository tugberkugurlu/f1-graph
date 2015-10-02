using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data.Common;
using System.Globalization;
using System.Linq;

namespace F1Graph.Etl
{
    public class Program
    {
        private readonly string _mySqlConnectionString = "Data Source=172.17.0.12;port=3306;Initial Catalog=f1;User Id=root;password=1234567890";
        private readonly string _mongoDbConnectionStr = "mongodb://172.17.0.16:27017";

        public void Main(string[] args)
        {
            var drivers = GetDrivers();
            var seasons = GetSeasons();

            foreach (var driver in drivers)
            {
                Console.WriteLine(driver.Firstname + " " + driver.Surname);
            }
        }

        private IEnumerable<Driver> GetDrivers()
        {
            using (DbConnection connection = new MySqlConnection(_mySqlConnectionString))
            using (DbCommand cmd = new MySqlCommand("SELECT * FROM drivers"))
            {
                connection.Open();
                cmd.Connection = connection;
                var reader = cmd.ExecuteReader();

                return reader.Select(r =>
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
                    if(DateTime.TryParse(dobStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out dob) == false)
                    {
                        Console.WriteLine("Couldn't parse the dob: " + dobStr + "; DriverId: " + driver.Id);
                    }
                    else
                    {
                        driver.DateOfBirth = dob;
                    }

                    return driver;

                }).ToList();
            }
        }

        private IEnumerable<Season> GetSeasons()
        {
            using (DbConnection connection = new MySqlConnection(_mySqlConnectionString))
            using (DbCommand cmd = new MySqlCommand("SELECT * FROM seasons"))
            {
                connection.Open();
                cmd.Connection = connection;
                var reader = cmd.ExecuteReader();

                return reader.Select(r => new Season
                {
                    Year = int.Parse(r["year"].ToString()),
                    Url = r["url"].ToString()
                }).ToList();
            }
        }
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
    }
}