using Dister4Net.Communication;
using Dister4Net.Helpers;
using Dister4Net.Modules.Communicators;
using Dister4Net.Modules.MessageHandlers;

namespace Dister4Net.Modules.Logging
{
    public class LogAggregator : MessageHandler
    {
        public override string ModuleName => "Log aggregator";

        void Log(MessagePacket packet)
        {
            var msg = $"{packet.SplittedTopic[0]}({packet.SplittedTopic[1]}): {packet.content}";
            service.GetModules<Logger>().ForEach(x => x.Log(msg));
        }
        protected override void Start()
            => AddMessageHandler("+/+/logs", Log, service.GetModules<Communicator>());
    }
}
