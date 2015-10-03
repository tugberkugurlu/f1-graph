using System;

namespace F1.Domain
{
    public class DriverEntity
    {
        public DriverEntity()
        {    
        }

        public string Id { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
    }
}
