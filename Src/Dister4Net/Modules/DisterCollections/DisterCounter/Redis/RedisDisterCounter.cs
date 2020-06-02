using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dister4Net.Attributes;
using Dister4Net.Modules.Connectors;
using StackExchange.Redis;

namespace Dister4Net.Modules.DisterCollections.DisterCounter.Redis
{
    [RequiresModule(typeof(RedisConnector))]
    public class RedisDisterCounter : DisterCounter
    {
        public override string ModuleName => $"Redis Dister Counter ({CollectionName})";
        IDatabase db;
        public RedisDisterCounter(string counterName)
        {
            CollectionName = counterName;
        }
        public RedisDisterCounter(string counterName, int step) : this(counterName)
        {
            Step = step;
        }

        public override IEnumerator<long> GetEnumerator() => GetValues().GetEnumerator();
        private IEnumerable<long> GetValues()
        {
            while (true) yield return Increment();
        }
        public override long Increment()
            => db.StringIncrement(CollectionName, Step);

        public override void Set(long value)
            => db.StringSet(CollectionName, value);

        protected override void Start()
            => db = service.GetModule<RedisConnector>().GetDatabase();
    }
}
