using System.Collections.Generic;
using System.Linq;
using Dister4Net.Modules;
using Dister4Net.Serialization;

namespace Dister4Net
{
    public abstract class DisterService
    {
        public abstract string ServiceName { get; }
        public abstract string NodeName { get; }
        internal ISerializer Serializer { get; set; }
        private List<Module> Modules { get; set; } = new List<Module>();
        public void AddModule(Module module)
            => Modules.Add(module);
        public IEnumerable<T> GetModules<T>() where T : Module 
            => Modules.Where(x => x is T).Select(x => x as T);
        public IEnumerable<Module> GetModules()
            => Modules;
        public T GetModule<T>() where T : Module
            => GetModules<T>().First();
    }
}
