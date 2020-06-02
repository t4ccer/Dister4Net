using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dister4Net.Modules.DisterCollections.DisterStack
{
    public abstract class DisterStack<T> : DisterCollection, IProducerConsumerCollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
    {
        public abstract int Count { get; }
        public abstract bool IsSynchronized { get; }
        public abstract object SyncRoot { get; }

        public abstract void CopyTo(Array array, int index);
        public abstract void CopyTo(T[] array, int index);
        public abstract T[] ToArray();
        public abstract bool TryAdd(T item);
        public abstract bool TryTake([MaybeNullWhen(true)] out T item);
        public abstract IEnumerator<T> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}
