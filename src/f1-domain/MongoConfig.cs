using System;
using System.Threading;
using F1.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Nte.Identity.Domain
{
    public static class MongoConfig
    {
        private static bool _initialized = false;
        private static object _initializationLock = new object();
        private static object _initializationTarget;

        /// <summary>
        /// Configures the object serialization rules and public conventions.
        /// </summary>
        /// <param name="registerConvetionsAction">
        /// An action which will be called to register the actions. This is accepted as a parameter to
        /// give the registration control to host so that it can regitser the rules only once in multiple
        /// MongoDB context cases.
        /// </param>
        public static void Configure(Action registerConvetionsAction)
        {
            EnsureConfigured(registerConvetionsAction);
        }

        private static void ConfigureImpl(Action registerConvetionsAction)
        {
            registerConvetionsAction();
            ConfigureEntities();
            ConfigureValueObjects();
        }

        private static void ConfigureEntities()
        {
            BsonClassMap.RegisterClassMap<BaseEntity>(cm =>
            {
               cm.AutoMap();
               cm.MapMember(c => c.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));
               cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            });
        }

        private static void ConfigureValueObjects()
        {
        }

        private static void EnsureConfigured(Action registerConvetionsAction)
        {
            LazyInitializer.EnsureInitialized(ref _initializationTarget, ref _initialized, ref _initializationLock, () =>
            {
                ConfigureImpl(registerConvetionsAction);
                return null;
            });
        }
    }
}
