using System;
using F1.Domain.ValueObjects;
using NodaTime;

namespace F1.Domain.Entities
{
    public class RaceEntity : BaseEntity
    {
        public RaceEntity(string name, SeasonReference season, ZonedDateTime raceDateTime, string url)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (season == null)
                throw new ArgumentNullException("season");
            if (url == null)
                throw new ArgumentNullException("url");

            Name = name;
            Season = season;
            RaceDateTime = raceDateTime;
            Url = url;
        }

        /// <summary>
        /// The name of the Grand Prix on the season for this race.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The season that the race was held in.
        /// </summary>
        public SeasonReference Season { get; private set; }

        /// <summary>
        /// The Circuit that the race was held.
        /// </summary>
        public CircuitReference Circuit { get; private set; }

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
