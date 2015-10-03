using System;

namespace F1.Domain.ValueObjects
{
    public class OccurrenceRecord
    {
        public OccurrenceRecord(DateTime occuredOn)
        {
            OccuredOn = occuredOn;
        }

        public DateTime OccuredOn { get; private set; }

        public static OccurrenceRecord Now()
        {
            return new OccurrenceRecord(DateTime.UtcNow);
        }
    }
}
