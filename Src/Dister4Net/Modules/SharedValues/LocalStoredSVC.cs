using System.Collections.Concurrent;

namespace Dister4Net.Modules.SharedValues
{
    public class LocalStoredSVC : SharedValuesController
    {
        public override string ModuleName => "Local stored SVC";

        ConcurrentDictionary<string, object> values;

        public override T Get<T>(string name)
            => (T)values[name];
        public override void Set(string name, object value)
            => values[name] = value;
        protected override void Start()
        {
            values = new ConcurrentDictionary<string, object>();
        }
    }
}
