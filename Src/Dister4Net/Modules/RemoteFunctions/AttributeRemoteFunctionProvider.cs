using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Dister4Net.Attributes;
using Dister4Net.Modules;

namespace Dister4Net.Modules.RemoteFunctions
{
    public class AttributeRemoteFunctionProvider : ModuleAdder
    {
        public override string ModuleName => "Attribute Remote Function Provider";

        protected override void Start()
        {
            var classes = Assembly.GetEntryAssembly().GetTypes();
            var methods = classes.Select(x => (type: x, methods: x.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).Where(m => m.GetCustomAttributes(typeof(RemoteFunctionProviderAttribute), true).Length > 0)))
                .Select(x => (x.type, methods: x.methods.Select(y => (method: y, y.GetCustomAttribute<RemoteFunctionProviderAttribute>().providerName))))
                .Where(x => x.methods.Count() > 0);

            foreach (var @class in methods)
            {
                foreach (var method in @class.methods)
                {
                    if (method.method.GetParameters().Count() > 1)
                        throw new Exception("RFP must contain 0 or 1 parameter. If you want to use more use tuple instead");

                    Func<object, object> func;
                    var inType = typeof(object);

                    if (method.method.GetParameters().Count() == 1)
                    {
                        inType = method.method.GetParameters().First().ParameterType;
                        var param = Expression.Parameter(inType);
                        var del = Expression.Lambda(Expression.Call(method.method, param), param).Compile();
                        func = arg => del.DynamicInvoke(arg);
                    }
                    else //if (method.method.GetParameters().Count() == 0)
                    {
                        var del = Expression.Lambda(Expression.Call(method.method)).Compile();
                        func = arg => del.DynamicInvoke();
                    }

                    var module = new RemoteFunctionProvider<object, object>(method.providerName, func, (inType, typeof(object)));
                    service.AddModule(module);
                    module.Initialize(serializer, service);
                }
            }

        }
    }
}
