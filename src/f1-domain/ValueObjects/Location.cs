using System;

namespace F1.Domain.ValueObjects
{
    /// <summary>
    /// Holds the coordintes as latitude and longitude.
    /// </summary>
    /// <remarks>
    /// For more information, refer to https://msdn.microsoft.com/en-us/library/aa578799.aspx and http://stackoverflow.com/q/6536232/463785.
    /// </remarks>
    public class Location
    {
        public Location(float latitude, float longitude)
        {
            if (latitude > 90 && latitude < -90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), latitude, "Value cannot be greater than 90 and lower than -90.");
            }

            if (longitude > 180 && longitude < -180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), longitude, "Value cannot be greater than 180 and lower than -180.");
            }

            Latitude = latitude;
            Longitude = longitude;
        }

        public float Latitude { get; private set; }
        public float Longitude { get; private set; }
    }
}
