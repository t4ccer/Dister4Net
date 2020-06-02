using System;

namespace Dister4Net.Attributes
{
    public class MessageHandlerAttribute : Attribute
    {
        internal string pattern;
        public MessageHandlerAttribute(string pattern)
        {
            this.pattern = pattern;
        }
    }
}
