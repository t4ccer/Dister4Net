using Dister4Net.Attributes;
using Dister4Net.Communication;
using Dister4Net.Helpers;
using Dister4Net.Modules.Communicators;

namespace Dister4Net.Modules.Logging
{
    [RequiresModule(typeof(Communicator))]
    public class LogSender : Logger
    {
        public override string ModuleName => "Log Sender";

        public override void Log(string message)
            => service.GetModules<Communicator>().ForEach(x => x.SendMessage(new MessagePacket { content = message, topic = $"{service.ServiceName}/{service.NodeName}/logs" }));

        protected override void Start() { }
    }
}
