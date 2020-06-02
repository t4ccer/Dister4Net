using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Dister4Net.Modules.DisterCollections.DisterCounter
{
    public abstract class DisterCounter : DisterCollection, IEnumerable<long>, IEnumerable
    {
        public int Step { get; set; } = 1;
        public abstract long Increment();
        public abstract void Set(long value);
        public abstract IEnumerator<long> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
