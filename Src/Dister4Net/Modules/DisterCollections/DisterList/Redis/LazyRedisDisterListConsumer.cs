using System.Collections.Generic;

namespace Dister4Net.Modules.DisterCollections.DisterList.Redis
{
    public class LazyRedisDisterListConsumer<T> : RedisDisterListConsumer<T>
    {
        public LazyRedisDisterListConsumer(string listName) : base(listName)
        {
        }

        public override IEnumerator<T> GetEnumerator()
            => new LazyDisterListEnumerator<T>(this);
    }
}
