using System;

namespace F1.Domain.Entities
{
    public class SeasonEntity : BaseEntity
    {
        public SeasonEntity(int year, string url)
        {
            if (url == null)
                throw new System.ArgumentNullException("url");

            if(year < 1950)
            {
                throw new ArgumentOutOfRangeException(nameof(year), year, "There is no Formula 1 season before 1950");
            }

            Year = year;
            Url = url;
        }

        public int Year { get; private set; }
        public string Url { get; private set; }
    }
}
