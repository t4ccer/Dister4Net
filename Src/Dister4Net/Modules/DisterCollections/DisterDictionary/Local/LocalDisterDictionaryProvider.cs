using System.Collections.Concurrent;
using System.Collections.Generic;
using Dister4Net.Modules.RemoteFunctions;

namespace Dister4Net.Modules.DisterCollections.DisterDictionary.Local
{
    public class LocalDisterDictionaryProvider<TKey, TValue> : DisterDictionary<TKey, TValue>
    {
        public override int StartPriority => 1;
        public override string ModuleName => $"Dister Dictionary Provider ({CollectionName})";

        private readonly ConcurrentDictionary<TKey, TValue> dictionary = new ConcurrentDictionary<TKey, TValue>();
        public LocalDisterDictionaryProvider(string dictionaryName)
        {
            this.CollectionName = dictionaryName;
        }

        public override void Set(TKey key, TValue value)
            => dictionary.AddOrUpdate(key, value, (a, b) => value);
        public override void Remove(TKey key)
            => dictionary.TryRemove(key, out var _);
        public override bool TryGet(TKey key, out TValue res)
        {
            var x = Get(key);
            if (x.Item1)
                res = x.Item2;
            else
                res = default;
            return x.Item1;
        }

        public (bool, TValue) Get(TKey key)
            => dictionary.TryGetValue(key, out var value) ? (true, value) : (false, default);


        protected override void Start()
        {
            service.AddModule(new RemoteFunctionProvider<(TKey, TValue), object>($"ddp-set-{CollectionName}", x => Set(x.Item1, x.Item2)));
            service.AddModule(new RemoteFunctionProvider<TKey, object>($"ddp-remove-{CollectionName}", Remove));
            service.AddModule(new RemoteFunctionProvider<TKey, (bool, TValue)>($"ddp-get-{CollectionName}", Get));
        }
    }
}

