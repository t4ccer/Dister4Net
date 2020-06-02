using System;

namespace Dister4Net.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ParallelLoopAttribute : Attribute
    {
        internal int count;
        public ParallelLoopAttribute(int count)
        {
            this.count = count;
        }
    }
}
