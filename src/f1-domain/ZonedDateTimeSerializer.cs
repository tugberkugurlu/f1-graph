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
            public const long DateTime = 1;
            public const long Ticks = 2;
            public const long Offset = 4;
            public const long TimeZoneId = 8;
        }

        private readonly SerializerHelper _helper;
        private readonly Int64Serializer _int64Serializer = new Int64Serializer();
        private readonly Int32Serializer _int32Serializer = new Int32Serializer();
        private readonly StringSerializer _stringSerializer = new StringSerializer();

        public ZonedDateTimeSerializer()
        {
            _helper = new SerializerHelper
            (
                new SerializerHelper.Member("DateTime", Flags.DateTime),
                new SerializerHelper.Member("Ticks", Flags.Ticks),
                new SerializerHelper.Member("Offset", Flags.Offset),
                new SerializerHelper.Member("TimeZoneId", Flags.TimeZoneId)
            );
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ZonedDateTime value)
        {
            var bsonWriter = context.Writer;
            var dateTimeOffset = value.ToDateTimeOffset();
            bsonWriter.WriteDateTime("DateTime", BsonUtils.ToMillisecondsSinceEpoch(dateTimeOffset.UtcDateTime);
            bsonWriter.WriteInt64("Ticks", dateTimeOffset.Ticks);
            bsonWriter.WriteInt32("Offset", (int)dateTimeOffset.Offset.TotalMinutes);
            bsonWriter.WriteString("TimeZoneId", value.Zone.Id);
            bsonWriter.WriteEndDocument();
        }

        public override ZonedDateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonReader = context.Reader;
            long ticks;
            TimeSpan offset;
            DateTimeZone timeZone;

            BsonType bsonType = bsonReader.GetCurrentBsonType();
            if (bsonType == BsonType.Document)
            {
                ticks = 0;
                offset = TimeSpan.Zero;
                timeZone = null;

                _helper.DeserializeMembers(context, (elementName, flag) =>
                {
                    switch (flag)
                    {
                        case Flags.DateTime: bsonReader.SkipValue(); break; // ignore value
                        case Flags.Ticks: ticks = _int64Serializer.Deserialize(context); break;
                        case Flags.Offset: offset = TimeSpan.FromMinutes(_int32Serializer.Deserialize(context)); break;
                        case Flags.TimeZoneId: timeZone = DateTimeZoneProviders.Tzdb[_stringSerializer.Deserialize(context)]; break;
                    }
                });

                var dateTimeOffset = new DateTimeOffset(ticks, offset);
                var instant = Instant.FromDateTimeOffset(dateTimeOffset);

                return new ZonedDateTime(instant, timeZone);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
