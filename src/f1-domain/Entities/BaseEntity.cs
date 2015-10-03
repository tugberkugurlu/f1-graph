using System;
using MongoDB.Bson;

namespace F1.Domain.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = ObjectId.GenerateNewId().ToString();
            CreatedOn = DateTime.UtcNow;
        }

        public string Id { get; private set; }
        public DateTime CreatedOn { get; private set; }
    }
}
