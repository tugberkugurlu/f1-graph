using System;
using NodaTime;

namespace F1.Domain.Entities
{
    public class RaceEntity : BaseEntity
    {
        public RaceEntity(string name, ZonedDateTime raceDateTime, string url)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (url == null)
                throw new ArgumentNullException("url");

            Name = name;
            RaceDateTime = raceDateTime;
            Url = url;
        }

        public string Name { get; private set; }

        /// <summary>
        /// Represents the date and time of the race in the time zone of the circuit on the date of the race.
        /// </summary>
        /// <remarks>
        /// UTC time is not stored as it's possible to get to that from this.
        /// </remarks>
        public ZonedDateTime RaceDateTime { get; private set; }

        public string Url { get; private set; }
    }
}
