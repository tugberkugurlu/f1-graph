using System.Threading;
using MongoDB.Bson.Serialization.Conventions;

namespace F1Graph.MySql.Etl
{
    public static class MongoConvetionsConfig
    {
        private static bool _initialized = false;
        private static object _initializationLock = new object();
        private static object _initializationTarget;

        public static void RegisterGlobalConventions()
        {
            EnsureRegistered();
        }

        private static void RegisterConventionsImpl()
        {
            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreIfNullConvention(false)
            };

            ConventionRegistry.Register("all", pack, type => true);
        }
        private static void EnsureRegistered()
        {
            LazyInitializer.EnsureInitialized(ref _initializationTarget, ref _initialized, ref _initializationLock, () =>
            {
                RegisterConventionsImpl();
                return null;
            });
        }
    }
}
