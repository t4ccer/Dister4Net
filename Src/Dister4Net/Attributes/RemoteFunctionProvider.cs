using System;

namespace Dister4Net.Attributes
{
    public class RemoteFunctionProviderAttribute : Attribute
    {
        internal string providerName;

        public RemoteFunctionProviderAttribute(string providerName)
        {
            this.providerName = providerName;
        }
    }
}
