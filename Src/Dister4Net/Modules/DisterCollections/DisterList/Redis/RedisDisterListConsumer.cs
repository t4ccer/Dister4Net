using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dister4Net.Attributes;
using Dister4Net.Helpers;
using Dister4Net.Modules.Connectors;
using StackExchange.Redis;

namespace Dister4Net.Modules.DisterCollections.DisterList.Redis
{
    [RequiresModule(typeof(RedisConnector))]
    public class RedisDisterListConsumer<T> : DisterList<T>
    {
        public override string ModuleName => $"Redis Dister List Consumer ({CollectionName})";

        private IDatabase db;

        public RedisDisterListConsumer(string listName)
        {
            this.CollectionName = listName ?? throw new ArgumentNullException(nameof(listName));
        }

        public override object this[int index]
        {
            get
            {
                var rv = db.ListGetByIndex(CollectionName, index);
                if (!rv.HasValue)
                    throw new ArgumentOutOfRangeException();
                return serializer.Deserialize<T>(Encoding.Default.GetString((byte[])rv.Box()));
            }
            set => Insert(index, value);
        }

        public override bool IsFixedSize
            => false;
        public override bool IsReadOnly
            => false;
        public override int Count
            => db.ListLength(CollectionName).ConvertTo<int>();
        public override bool IsSynchronized
            => true;
        public override object SyncRoot
            => new object();


        public override int Add(object value)
            => db.ListRightPush(CollectionName, serializer.Serialize(value)).ConvertTo<int>() - 1;
        public override void Add(T item)
            => Add((object)item);

        public override void Clear() => db.ListTrim(CollectionName, 0, 0);

        public override bool Contains(object value) => GetAll().Contains((T)value);
        public override bool Contains(T item) => GetAll().Contains(item);

        public override void CopyTo(Array array, int index) => GetAll().ToArray().CopyTo(array, index);
        public override void CopyTo(T[] array, int arrayIndex) => GetAll().ToArray().CopyTo(array, arrayIndex);

        public override IEnumerator<T> GetEnumerator()
            => GetAll().GetEnumerator();

        public override int IndexOf(object value) => GetAll().ToList().IndexOf((T)value);
        public override int IndexOf(T item) => GetAll().ToList().IndexOf(item);

        public override void Insert(int index, object value) => db.ListSetByIndex(CollectionName, index, serializer.Serialize(value));
        public override void Insert(int index, T item) => Insert(index, (object)item);

        public override void Remove(object value) => db.ListRemove(CollectionName, serializer.Serialize(value));
        public override bool Remove(T item) => db.ListRemove(CollectionName, serializer.Serialize(item)) > 0;
        public override void RemoveAt(int index) => Remove(this[index]);

        private IEnumerable<T> GetAll()
            => db.ListRange(CollectionName).Select(x => serializer.Deserialize<T>(Encoding.Default.GetString((byte[])x.Box())));

        protected override void Start()
            => db = service.GetModule<RedisConnector>().GetDatabase();
    }
}
