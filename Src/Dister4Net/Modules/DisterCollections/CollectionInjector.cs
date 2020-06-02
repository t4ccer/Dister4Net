using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Dister4Net.Helpers;

namespace Dister4Net.Modules.DisterCollections
{
    public class CollectionInjector : Module
    {
        public override string ModuleName => "Collection injector";

        protected override void Start()
        {
            service.GetType().GetRuntimeFields().ForEach(x =>
            {
                if(x.GetValue(service) == null)
                    x.SetValue(service, service.GetModules<DisterCollection>().FirstOrDefault(y => y.CollectionName == x.Name));
            });
        }
    }
}
