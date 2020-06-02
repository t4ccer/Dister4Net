using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Dister4Net.Communication;
using Dister4Net.Helpers;
using Dister4Net.Modules.Communicators;
using Dister4Net.Modules.MessageHandlers;

namespace Dister4Net.Modules.RemoteFunctions
{
    public class RemoteFunctionProvider<TIn, TOut> : MessageHandler
    {
        public override string ModuleName => $"Remote Function Provider ({ProviderName})";
        public virtual string ProviderName { get; set; }
        Func<object, object> function;
        List<Communicator> communicators;
        (Type tIn, Type tOut) types;

        public RemoteFunctionProvider(string name, Func<TIn, TOut> function)
        {
            ProviderName = name;
            this.function = arg => function((TIn)arg);
            types = (function.GetMethodInfo().GetParameters()[0].ParameterType, function.GetMethodInfo().ReturnType);
        }
        public RemoteFunctionProvider(string name, Func<object, object> function, (Type tIn, Type tOut) types)
        {
            ProviderName = name;
            this.function = function;
            this.types = types;
        }
        public RemoteFunctionProvider(string name, Action<TIn> action)
        {
            ProviderName = name;
            function = x => { action((TIn)x); return null; };
            types = (action.GetMethodInfo().GetParameters()[0].ParameterType, null);
        }
        public RemoteFunctionProvider(string name, Action<object> action, Type tIn)
        {
            ProviderName = name;
            function = x => { action(x); return null; };
            types = (tIn, null);
        }
        public RemoteFunctionProvider(string name, Func<TOut> function)
        {
            ProviderName = name;
            this.function = x => function();
            types = (null, function.GetMethodInfo().ReturnType);
        }
        public RemoteFunctionProvider(string name, Func<TOut> function, Type tOut)
        {
            ProviderName = name;
            this.function = x => function();
            types = (null, tOut);
        }

        void Handle(MessagePacket packet)
        {
            var input = (types.tIn != null) ? serializer.Deserialize(packet.content, types.tIn) : default;
            var result = function.Invoke(input);

            if (types.tOut == null)
                return;

            var response = new MessagePacket
            {
                content = serializer.Serialize(result),
                topic = $"{service.ServiceName}/{service.NodeName}/rfp/response/{ProviderName}/{packet.SplittedTopic.Last()}"
            };
            communicators.ForEach(c => c.SendMessage(response));
        }
        protected override void Start()
        {
            communicators = service.GetModules<Communicator>().ToList();
            AddMessageHandler($"+/+/rfc/request/{ProviderName}/+", Handle, communicators);
        }
    }
}
