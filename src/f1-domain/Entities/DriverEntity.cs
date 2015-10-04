using System;
using F1.Domain.ValueObjects;
using MongoDB.Bson;

namespace F1.Domain.Entities
{
    public class DriverEntity : BaseEntity
    {
        public DriverEntity(string originId, string firstName, string lastName, string nationality, string url)
            : this(originId, firstName, lastName, nationality, url, null)
        {
        }

        public DriverEntity(string originId, string firstName, string lastName, string nationality, string url, DateTime dateOfBirth)
            : this(originId, firstName, lastName, nationality, url, new OccurrenceRecord(dateOfBirth))
        {
        }

        private DriverEntity(string originId, string firstName, string lastName, string nationality, string url, OccurrenceRecord dateOfBirth)
        {
            if (originId == null)
                throw new ArgumentNullException("originId");
            if (firstName == null)
                throw new ArgumentNullException("firstName");
            if (lastName == null)
                throw new ArgumentNullException("lastName");
            if (nationality == null)
                throw new ArgumentNullException("nationality");
            if (url == null)
                throw new ArgumentNullException("url");

            OriginId = originId;
            Firstname = firstName;
            Lastname = lastName;
            Nationality = nationality;
            Url = url;
            DateOfBirth = dateOfBirth;
        }

        public string OriginId { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Nationality { get; private set; }
        public string Url { get; private set; }
        public OccurrenceRecord DateOfBirth { get; private set; }
    }
}
