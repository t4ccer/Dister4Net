using System.Collections.Generic;
using Dister4Net.Modules.RemoteFunctions;

namespace Dister4Net.Modules.DisterCollections.DisterDictionary.Local
{
    public class LocalDisterDictionaryConsumer<TKey, TValue> : DisterDictionary<TKey, TValue>
    {
        public override int StartPriority => 1;
        public override string ModuleName => $"Dister Dictionary Consumer ({dictionaryName})";
        string dictionaryName;
        public LocalDisterDictionaryConsumer(string dictionaryName)
            => this.dictionaryName = dictionaryName;

        RemoteFunctionConsumer<(TKey, TValue), object> setRFC;
        RemoteFunctionConsumer<TKey, object> removeRFC;
        RemoteFunctionConsumer<TKey, (bool, TValue)> getRFC;

        public override void Set(TKey key, TValue value)
        {
            setRFC.Get((key, value));
        }
        public override void Remove(TKey key)
        {
            removeRFC.Get(key);
        }
        public override bool TryGet(TKey key, out TValue value)
        {
            var res = getRFC.Get(key);
            if (res.Item1)
                value = res.Item2;
            else
                value = default;
            return res.Item1;
        }



        protected override void Start()
        {
            service.AddModule(setRFC = new RemoteFunctionConsumer<(TKey, TValue), object>($"ddp-set-{dictionaryName}"));
            service.AddModule(removeRFC = new RemoteFunctionConsumer<TKey, object>($"ddp-remove-{dictionaryName}"));
            service.AddModule(getRFC = new RemoteFunctionConsumer<TKey, (bool, TValue)>($"ddp-get-{dictionaryName}"));
        }
    }
}