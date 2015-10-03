using System;
using F1.Domain.ValueObjects;
using MongoDB.Bson;

namespace F1.Domain
{
    public class DriverEntity
    {
        public DriverEntity(string firstName, string lastName, string nationality, string url)
            : this(firstName, lastName, nationality, url, null)
        {
        }

        public DriverEntity(string firstName, string lastName, string nationality, string url, DateTime dateOfBirth)
            : this(firstName, lastName, nationality, url, new OccurrenceRecord(dateOfBirth))
        {
        }

        private DriverEntity(string firstName, string lastName, string nationality, string url, OccurrenceRecord dateOfBirth)
        {
            if (firstName == null)
                throw new ArgumentNullException("firstName");
            if (lastName == null)
                throw new ArgumentNullException("lastName");
            if (nationality == null)
                throw new ArgumentNullException("nationality");
            if (url == null)
                throw new ArgumentNullException("url");

            Id = ObjectId.GenerateNewId().ToString();
            Firstname = firstName;
            Lastname = lastName;
            Nationality = nationality;
            Url = url;
            DateOfBirth = dateOfBirth;
        }

        public string Id { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Nationality { get; private set; }
        public string Url { get; private set; }
        public OccurrenceRecord DateOfBirth { get; private set; }
    }
}
