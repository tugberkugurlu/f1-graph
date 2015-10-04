using F1.Domain.ValueObjects;

namespace F1.Domain.Entities
{
    public class CircuitEntity : BaseEntity
    {
        public string OriginId { get; private set; }
        public string Name { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }
        public Location Location { get; private set; }
        public string Url { get; private set; }
    }
}
