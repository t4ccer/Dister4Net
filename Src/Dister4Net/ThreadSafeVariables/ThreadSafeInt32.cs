using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dister4Net.ThreadSafeVariables
{
    public class ThreadSafeInt32
    {
        int val;
        public ThreadSafeInt32(int val)
        {
            this.val = val;
        }
        public int Get() => val;
        public int Increment()
            => Interlocked.Increment(ref val);
        public void Decrement()
            => Interlocked.Decrement(ref val);
        public void Add(int value)
            => Interlocked.Add(ref val, value);
        public void Sub(int value)
            => Interlocked.Add(ref val, -value);

        public static implicit operator ThreadSafeInt32(int value)
            => new ThreadSafeInt32(value);
    }
}
