using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Timers;
using Dister4Net.Communication;
using Dister4Net.Helpers;
using Dister4Net.Modules.Communicators;
using Dister4Net.Modules.MessageHandlers;

namespace Dister4Net.Modules.Heartbeat
{
    public class HeartbeatAggregator : MessageHandler
    {
        public override string ModuleName => "Heartbeat aggregator";

        private readonly ConcurrentDictionary<string, DateTime> connectedServices = new ConcurrentDictionary<string, DateTime>();
        private readonly Timer timer;
        public IEnumerable<KeyValuePair<string, DateTime>> HeartbeatStatus => connectedServices.ToImmutableList();


        public HeartbeatAggregator(double interval = 5000)
        {
            timer = new Timer(interval);
            timer.Elapsed += (a, b) => service.GetModules<Communicator>().ForEach(x => x.SendMessage(new MessagePacket { topic = new TopicBuilder().Excatly(service.ServiceName).Excatly(service.NodeName).Excatly("heartbeat").Excatly("request").Build() }));
        }
        void Handler(MessagePacket messagePacket)
        {
            var serviceName = messagePacket.SplittedTopic[0] + "/" + messagePacket.SplittedTopic[1];
            connectedServices[serviceName] = DateTime.UtcNow;
        }
        protected override void Start()
        {
            AddMessageHandler(new TopicBuilder().One().One().Excatly("heartbeat").Excatly("response").Build(), Handler, service.GetModules<Communicator>());
            timer.Start();
        }
    }
}
