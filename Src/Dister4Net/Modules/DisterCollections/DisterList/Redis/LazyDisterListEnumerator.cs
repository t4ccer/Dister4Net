using System;
using System.Collections;
using System.Collections.Generic;
using Dister4Net.ThreadSafeVariables;

namespace Dister4Net.Modules.DisterCollections.DisterList.Redis
{
    public class LazyDisterListEnumerator<T> : IEnumerator<T>
    {
        public T Current { get; set; }
        object IEnumerator.Current => Current;

        private ThreadSafeInt32 i = -1;
        private readonly LazyRedisDisterListConsumer<T> list;

        public LazyDisterListEnumerator(LazyRedisDisterListConsumer<T> list)
        {
            this.list = list ?? throw new ArgumentNullException(nameof(list));
        }

        public void Dispose() { }
        public bool MoveNext()
        {
            var a = i.Increment();
            if (a < list.Count)
            {
                try
                {
                    Current = (T)list[a];
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }
            }
            return false;
        }
        public void Reset() => i = -1;
    }
}
