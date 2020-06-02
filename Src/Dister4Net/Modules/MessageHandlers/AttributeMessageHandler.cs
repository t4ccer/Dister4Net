using System.Linq;
using Dister4Net.Helpers;
using System;
using Dister4Net.Attributes;
using Dister4Net.Modules.Communicators;

namespace Dister4Net.Modules.MessageHandlers
{
    public class AttributeMessageHandler : MessageHandler
    {
        public override string ModuleName => "Attribute Message Handler";

        protected override void Start()
        {
            var handlers = service
                .GetType()
                .GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(MessageHandlerAttribute), true).Length > 0)
                .Select(x => (((MessageHandlerAttribute)x.GetCustomAttributes(true).First(x => x.GetType() == typeof(MessageHandlerAttribute))).pattern, method: x));

            handlers
                .Where(x => x.method.GetParameters().Length > 1)
                .ForEach(x => throw new Exception($"Handler method ({x.method.Name}) cannot have more than 1 parameter"));

            handlers
            .Select(x => (x.pattern, x.method, parameters: x.method.GetParameters()))
            .ForEach(x => AddMessageHandler(
                x.pattern,
                (msg) => x.method.Invoke(service, x.parameters.Select(x => serializer.Deserialize(msg.content, x.ParameterType)).Take(1).ToArray()),
                service.GetModules<Communicator>()
            ));
        }
    }
}
