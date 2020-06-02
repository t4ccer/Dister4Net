using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Dister4Net.Attributes;
using Dister4Net.Helpers;
using Dister4Net.Modules.Connectors;
using StackExchange.Redis;

namespace Dister4Net.Modules.DisterCollections.DisterStack.Redis
{
    [RequiresModule(typeof(RedisConnector))]
    public class RedisDisterStackConsumer<T> : DisterStack<T>
    {
        public override string ModuleName => $"Redis Dister Stack Consumer ({CollectionName})";
        private IDatabase db;
        public RedisDisterStackConsumer(string stackName)
        {
            this.CollectionName = stackName;
        }
        public override int Count
            => db.ListLength(CollectionName).ConvertTo<int>();
        public override bool IsSynchronized => true;
        public override object SyncRoot => new object();


        public override void CopyTo(Array array, int index) => GetAll().ToArray().CopyTo(array, index);
        public override void CopyTo(T[] array, int index) => GetAll().ToArray().CopyTo(array, index);

        public override IEnumerator<T> GetEnumerator() => GetAll().GetEnumerator();

        public override T[] ToArray() => GetAll().ToArray();

        public override bool TryAdd(T item)
            => db.ListRightPush(CollectionName, serializer.Serialize(item)).ConvertTo<int>() > 0;
        public override bool TryTake([MaybeNullWhen(true)] out T item)
        {
            var rv = (byte[])db.ListRightPop(CollectionName).Box();
            if (rv == null)
                item = default;
            else
                item = serializer.Deserialize<T>(Encoding.Default.GetString(rv));
            return rv != null;
        }
        private IEnumerable<T> GetAll()
            => db.ListRange(CollectionName).Select(x => serializer.Deserialize<T>(Encoding.Default.GetString((byte[])x.Box())));


        protected override void Start()
            => db = service.GetModule<RedisConnector>().GetDatabase();
    }
}
