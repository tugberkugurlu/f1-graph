using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using NodaTime;
using MongoDB.Bson.IO;

namespace F1.Domain
{
    /// <remarks>
    /// See https://github.com/mongodb/mongo-csharp-driver/blob/master/src/MongoDB.Bson/Serialization/Serializers/DateTimeOffsetSerializer.cs for
    /// a smilar sample.
    /// </remarks>
    public class ZonedDateTimeSerializer : SerializerBase<ZonedDateTime>
    {
        private static class Flags
        {
            public const long UnixEpoch = 1;
            public const long TimeZoneId = 2;
        }

        private readonly SerializerHelper _helper;
        private readonly Int64Serializer _int64Serializer = new Int64Serializer();
        private readonly StringSerializer _stringSerializer = new StringSerializer();

        public ZonedDateTimeSerializer()
        {
            _helper = new SerializerHelper
            (
                new SerializerHelper.Member("unixEpoch", Flags.UnixEpoch),
                new SerializerHelper.Member("timeZoneId", Flags.TimeZoneId)
            );
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ZonedDateTime value)
        {
            var bsonWriter = context.Writer;
            var dateTimeOffset = value.ToDateTimeOffset();
            bsonWriter.WriteStartDocument();
            bsonWriter.WriteInt64("unixEpoch", BsonUtils.ToMillisecondsSinceEpoch(dateTimeOffset.UtcDateTime));
            bsonWriter.WriteString("timeZoneId", value.Zone.Id);
            bsonWriter.WriteEndDocument();
        }

        public override ZonedDateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonReader = context.Reader;
            long epoch;
            DateTimeZone timeZone;

            BsonType bsonType = bsonReader.GetCurrentBsonType();
            if (bsonType == BsonType.Document)
            {
                epoch = 0;
                timeZone = null;

                _helper.DeserializeMembers(context, (elementName, flag) =>
                {
                    switch (flag)
                    {
                        case Flags.UnixEpoch: epoch = _int64Serializer.Deserialize(context); break;
                        case Flags.TimeZoneId: timeZone = DateTimeZoneProviders.Tzdb[_stringSerializer.Deserialize(context)]; break;
                    }
                });

                var instant = Instant.FromMillisecondsSinceUnixEpoch(epoch);
                return new ZonedDateTime(instant, timeZone);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
