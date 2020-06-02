using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Dister4Net.Communication;
using Dister4Net.Helpers;
using Dister4Net.Modules.Communicators;
using Dister4Net.Modules.MessageHandlers;

namespace Dister4Net.Modules.RemoteFunctions
{
    public abstract class RemoteFunctionConsumer : MessageHandler
    {
        public abstract string ConsumerName { get; set; }
    }

    public class RemoteFunctionConsumer<TOut> : RemoteFunctionConsumer<object, TOut>
    {
        public RemoteFunctionConsumer(string name) : base(name)
        {
        }
        public TOut Get()
            => Get(null);

    }
    public class RemoteFunctionConsumer<TIn, TOut> :  RemoteFunctionConsumer
    {
        public override string ModuleName => $"Remote Function Consumer ({ConsumerName})";
        public override string ConsumerName { get; set; }

        List<Communicator> communicators;
        ConcurrentDictionary<string, MessagePacket> responses = new ConcurrentDictionary<string, MessagePacket>();

        public RemoteFunctionConsumer(string name)
        {
            ConsumerName = name;
        }
        public TOut Get(TIn input)
        {
            var id = Guid.NewGuid().ToString();
            var packet = new MessagePacket
            {
                content = serializer.Serialize(input),
                topic = $"{service.ServiceName}/{service.NodeName}/rfc/request/{ConsumerName}/{id}"
            };
            communicators.ForEach(c => c.SendMessage(packet));

            LinqHelpers.WaitUntil(() => responses.ContainsKey(id));

            responses.Remove(id, out var response);
            return serializer.Deserialize<TOut>(response.content);
        }
        protected override void Start()
        {
            communicators = service.GetModules<Communicator>().ToList();
            AddMessageHandler($"+/+/rfp/response/{ConsumerName}/+", x => responses.AddOrUpdate(x.SplittedTopic.Last(), x, (a, b) => x), communicators);
        }
    }
}
