using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dister4Net.Modules.DisterCollections.DisterQueue
{
    public abstract class DisterQueue<T> : DisterCollection, IProducerConsumerCollection<T>, IEnumerable<T>, IReadOnlyCollection<T>, ICollection, IEnumerable
    {
        public abstract int Count { get; }
        public abstract bool IsSynchronized { get; }
        public abstract object SyncRoot { get; }


        public abstract void CopyTo(T[] array, int index);
        public abstract T[] ToArray();
        public abstract bool TryAdd(T item);
        public abstract bool TryTake([MaybeNullWhen(true)] out T item);
        public abstract IEnumerator<T> GetEnumerator();
        public abstract void CopyTo(Array array, int index);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
