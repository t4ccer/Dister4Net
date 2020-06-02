using System;

namespace Dister4Net.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EndlessAttribute : Attribute
    {
        internal int interval = 0;

        public EndlessAttribute(int interval = 0)
        {
            this.interval = interval;
        }
    }
}
