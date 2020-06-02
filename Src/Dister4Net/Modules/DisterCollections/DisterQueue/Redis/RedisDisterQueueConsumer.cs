using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Dister4Net.Attributes;
using Dister4Net.Helpers;
using Dister4Net.Modules.Connectors;
using StackExchange.Redis;

namespace Dister4Net.Modules.DisterCollections.DisterQueue.Redis
{
    [RequiresModule(typeof(RedisConnector))]
    public class RedisDisterQueueConsumer<T> : DisterQueue<T>
    {
        public override string ModuleName => $"Redis Dister Queue Consumer ({CollectionName})";
        private IDatabase db;

        public RedisDisterQueueConsumer(string queueName)
        {
            this.CollectionName = queueName;
        }

        public override int Count 
            => db.ListLength(CollectionName).ConvertTo<int>();
        public override bool IsSynchronized => true;
        public override object SyncRoot => new object();

        public override void CopyTo(T[] array, int index) => GetAll().ToArray().CopyTo(array, index);
        public override T[] ToArray() => GetAll().ToArray();
        public override bool TryAdd(T item)
        {
            db.ListRightPush(CollectionName, serializer.Serialize(item));
            return true;
        }
        public override bool TryTake([MaybeNullWhen(true)] out T item)
        {
            var rv = (byte[])db.ListLeftPop(CollectionName).Box();
            item = rv == null ? default : serializer.Deserialize<T>(Encoding.Default.GetString(rv));
            return rv != null;
        }
        public override IEnumerator<T> GetEnumerator() => GetAll().GetEnumerator();
        public override void CopyTo(Array array, int index) => GetAll().ToArray().CopyTo(array, index);

        private IEnumerable<T> GetAll()
         => db.ListRange(CollectionName).Select(x => serializer.Deserialize<T>(Encoding.Default.GetString((byte[])x.Box())));


        protected override void Start()
            => db = service.GetModule<RedisConnector>().GetDatabase();

    }

}
