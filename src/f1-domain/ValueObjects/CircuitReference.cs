namespace F1.Domain.ValueObjects
{
    public class CircuitReference
    {
        public CircuitReference(string name, string city, string country, Location location)
        {
            if (name == null)
                throw new System.ArgumentNullException("name");
            if (city == null)
                throw new System.ArgumentNullException("city");
            if (country == null)
                throw new System.ArgumentNullException("country");
            if (location == null)
                throw new System.ArgumentNullException("location");

            Name = name;
            City = city;
            Country = country;
            Location = location;
        }

        public string Name { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }
        public Location Location { get; private set; }
    }
}
