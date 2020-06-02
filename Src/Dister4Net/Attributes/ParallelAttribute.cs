using System;

namespace Dister4Net.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ParallelAttribute : Attribute
    {
        internal int count;
        public ParallelAttribute(int count)
        {
            this.count = count;
        }
    }
}
