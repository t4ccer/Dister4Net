using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dister4Net.ThreadSafeVariables;
using Xunit;

namespace Dister4Net.Tests
{
    public class ThreadSafeInt32
    {
        [Theory]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void IncrementStressTest(int n)
        {
            ThreadSafeVariables.ThreadSafeInt32 x = 0;
            Parallel.For(0, n, i => x.Increment());
            Assert.Equal(n, x.Get());
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void DecrementStressTest(int n)
        {
            ThreadSafeVariables.ThreadSafeInt32 x = n;
            Parallel.For(0, n, i => x.Decrement());
            Assert.Equal(0, x.Get());
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void AddStressTest(int n)
        {
            ThreadSafeVariables.ThreadSafeInt32 x = 0;
            Parallel.For(0, n, i => x.Add(2));
            Assert.Equal(n*2, x.Get());
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void SubStressTest(int n)
        {
            ThreadSafeVariables.ThreadSafeInt32 x = 2*n;
            Parallel.For(0, n, i => x.Sub(2));
            Assert.Equal(0, x.Get());
        }
    }
}
