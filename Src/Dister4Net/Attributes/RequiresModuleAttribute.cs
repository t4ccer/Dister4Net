using System;
using System.Collections.Generic;
using System.Text;

namespace Dister4Net.Attributes
{
    class RequiresModuleAttribute : Attribute
    {
        internal readonly Type type;
        public RequiresModuleAttribute(Type type)
        {
            this.type = type;
        }
    }
}
