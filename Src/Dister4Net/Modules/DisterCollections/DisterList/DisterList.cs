using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Dister4Net.Modules.DisterCollections.DisterList
{
    public abstract class DisterList<T> : DisterCollection, ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList
    {
        public abstract object this[int index] { get; set; }

        T IReadOnlyList<T>.this[int index] => (T)this[index];

        T IList<T>.this[int index] { get => (T)this[index]; set => this[index] = value; }

        public abstract bool IsFixedSize { get; }
        public abstract bool IsReadOnly { get; }
        public abstract int Count { get; }
        public abstract bool IsSynchronized { get; }
        public abstract object SyncRoot { get; }

        public abstract int Add(object value);
        public abstract void Add(T item);
        public abstract void Clear();
        public abstract bool Contains(object value);
        public abstract bool Contains(T item);
        public abstract void CopyTo(Array array, int index);
        public abstract void CopyTo(T[] array, int arrayIndex);
        public abstract IEnumerator<T> GetEnumerator();

        //public abstract IEnumerator GetEnumerator()
        public abstract int IndexOf(object value);
        public abstract int IndexOf(T item);
        public abstract void Insert(int index, object value);
        public abstract void Insert(int index, T item);
        public abstract void Remove(object value);
        public abstract bool Remove(T item);
        public abstract void RemoveAt(int index);
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
        //IEnumerator<T> IEnumerable<T>.GetEnumerator()
    }
}
