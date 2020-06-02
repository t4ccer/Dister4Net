using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Dister4Net.Helpers;

namespace Dister4Net.Modules.RemoteFunctions
{
    public class RFCInjector : Module
    {
        public override string ModuleName => "RFC injector";

        protected override void Start()
        {
            service.GetType().GetRuntimeFields().ForEach(x =>
            {
                if (x.GetValue(service) == null)
                    x.SetValue(service, service.GetModules<RemoteFunctionConsumer>().FirstOrDefault(y => y.ConsumerName == x.Name));
            });
        }
    }
}
