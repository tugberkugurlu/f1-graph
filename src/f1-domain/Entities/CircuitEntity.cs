using F1.Domain.ValueObjects;

namespace F1.Domain.Entities
{
    public class CircuitEntity : BaseEntity
    {
        public CircuitEntity(string originId, string name, string city, string country, Location location, string url)
        {
            if (originId == null)
                throw new System.ArgumentNullException("originId");
            if (name == null)
                throw new System.ArgumentNullException("name");
            if (city == null)
                throw new System.ArgumentNullException("city");
            if (country == null)
                throw new System.ArgumentNullException("country");
            if (location == null)
                throw new System.ArgumentNullException("location");
            if (url == null)
                throw new System.ArgumentNullException("url");

            OriginId = originId;
            Name = name;
            City = city;
            Country = country;
            Location = location;
            Url = url;
        }

        public string OriginId { get; private set; }
        public string Name { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }
        public Location Location { get; private set; }
        public string Url { get; private set; }
    }
}
