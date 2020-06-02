using System.Collections.Generic;

namespace Dister4Net.Modules.DisterCollections.DisterDictionary
{
    public abstract class DisterDictionary<TKey, TValue> : DisterCollection
    {
        public abstract void Set(TKey key, TValue value);
        public abstract void Remove(TKey key);
        public abstract bool TryGet(TKey key, out TValue res);

        public TValue this[TKey key]
        {
            get => TryGet(key, out var res) ? res : throw new KeyNotFoundException();
            set => Set(key, value);
        }
    }
}
