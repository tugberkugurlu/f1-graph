using NodaTime;

namespace F1.Domain.Entities
{
    public class RaceEntity : BaseEntity
    {
        public string Name { get; private set; }

        /// <summary>
        /// Represents the date and time of the race in the time zone of the circuit on the date of the race.
        /// </summary>
        /// <remarks>
        /// UTC time is not stored as it's possible to get to that from this.
        /// </remarks>
        public ZonedDateTime RaceDateAndTime { get; private set; }

        public string Url { get; private set; }
    }
}
