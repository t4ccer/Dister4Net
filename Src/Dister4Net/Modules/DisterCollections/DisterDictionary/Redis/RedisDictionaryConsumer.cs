using System.Text;
using Dister4Net.Attributes;
using Dister4Net.Modules.Connectors;
using StackExchange.Redis;

namespace Dister4Net.Modules.DisterCollections.DisterDictionary.Redis
{
    [RequiresModule(typeof(RedisConnector))]
    public class RedisDictionaryConsumer<TKey, TValue> : DisterDictionary<TKey, TValue>
    {
        public override string ModuleName => $"Redis Dictionary Consumer ({CollectionName})";

        private IDatabase db;
        public RedisDictionaryConsumer(string dictionaryName)
        {
            this.CollectionName = dictionaryName;
        }
        protected override void Start()
            => db = service.GetModule<RedisConnector>().GetDatabase();
        public override void Set(TKey key, TValue value)
            => db.HashSet(CollectionName, serializer.Serialize(key), serializer.Serialize(value));
        public override void Remove(TKey key)
            => db.HashDelete(CollectionName, serializer.Serialize(key));
        public override bool TryGet(TKey key, out TValue res)
        {
            var rv = db.HashGet(CollectionName, serializer.Serialize(key));
            var value = (byte[])rv.Box();
            if (value == null)
                res = default;
            else
                res = serializer.Deserialize<TValue>(Encoding.Default.GetString(value));
            return rv.HasValue;
        }
    }
}
