using NodaTime;

namespace F1.Domain.Entities
{
    public class RaceEntity : BaseEntity
    {
        public string Name { get; private set; }
        public LocalDate RaceDate { get; private set; }
        public LocalTime RaceTimeInUtc { get; private set; }
        public LocalTime RaceTimeInLocal { get; private set; }
        public string Url { get; private set; }
    }
}
