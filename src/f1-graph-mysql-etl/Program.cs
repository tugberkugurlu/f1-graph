using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data.Common;

namespace F1Graph.Etl
{
    public class Program
    {
        public void Main(string[] args)
        {
            string connectionString = "Data Source=172.17.0.12;port=3306;Initial Catalog=f1;User Id=root;password=1234567890";
            using (DbConnection connection = new MySqlConnection(connectionString))
            using (DbCommand cmd = new MySqlCommand("SELECT * FROM seasons"))
            {
                connection.Open();
                cmd.Connection = connection;
                var reader = cmd.ExecuteReader();

                var seasons = reader.Select(r => new Season
                {
                    Year = int.Parse(r["year"].ToString()),
                    Url = r["url"].ToString()
                });

                foreach(var season in seasons)
                {
                    Console.WriteLine(season.Year);
                }
            }
        }
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
