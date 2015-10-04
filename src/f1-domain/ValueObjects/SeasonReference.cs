using System;

namespace F1.Domain.ValueObjects
{
    public class SeasonReference
    {
        public SeasonReference(string id, int year)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            Id = id;
            Year = year;
        }

        public string Id { get; private set; }
        public int Year { get; private set; }
    }
}
