using System;

namespace F1.Domain.Entities
{
    public class ConstructorEntity : BaseEntity
    {
        public ConstructorEntity(string originId, string name, string nationality, string url)
        {
            if (originId == null)
                throw new ArgumentNullException("originId");
            if (name == null)
                throw new ArgumentNullException("name");
            if (nationality == null)
                throw new ArgumentNullException("nationality");
            if (url == null)
                throw new ArgumentNullException("url");

            OriginId = originId;
            Name = name;
            Nationality = nationality;
            Url = url;
        }

        public string OriginId { get; private set; }
        public string Name { get; private set; }
        public string Nationality { get; private set; }
        public string Url { get; private set; }
    }
}
