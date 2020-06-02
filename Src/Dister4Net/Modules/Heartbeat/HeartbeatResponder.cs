using Dister4Net.Communication;
using Dister4Net.Helpers;
using Dister4Net.Modules.Communicators;
using Dister4Net.Modules.MessageHandlers;

namespace Dister4Net.Modules.Heartbeat
{
    public class HeartbeatResponder : MessageHandler
    {
        public override string ModuleName => "Heartbeat responder";

        void Handler(MessagePacket messagePacket)
        {
            var topic = new TopicBuilder().Excatly(service.ServiceName).Excatly(service.NodeName).Excatly("heartbeat").Excatly("response").Build();
            service.GetModules<Communicator>().ForEach(x => x.SendMessage(new MessagePacket { topic = topic }));
        }
        protected override void Start()
        {
            AddMessageHandler(new TopicBuilder().One().One().Excatly("heartbeat").Excatly("request").Build(), Handler, service.GetModules<Communicator>());
        }
    }
}
