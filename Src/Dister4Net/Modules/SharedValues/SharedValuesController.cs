using System;
using System.Collections.Generic;
using System.Text;
using Dister4Net.Modules;
using Dister4Net.Modules.MessageHandlers;

namespace Dister4Net.Modules.SharedValues
{
    public abstract class SharedValuesController : MessageHandler
    {
        public abstract T Get<T>(string name);
        public abstract void Set(string name, object value);
    }
}
