using System;
using System.Collections.Generic;
using System.Text;
using Dister4Net.Attributes;
using Dister4Net.Communication;
using Dister4Net.Helpers;
using Dister4Net.Modules;
using Dister4Net.Modules.Communicators;

namespace Dister4Net.Modules.MessageHandlers
{
    [RequiresModule(typeof(Communicator))]
    public abstract class MessageHandler : Module
    {
        public void HandleMessage(string topicPattern, MessagePacket messagePacket, Action<MessagePacket> handler)
            => messagePacket.MatchTopic(topicPattern).IfTrue(() => handler(messagePacket));
        public void AddMessageHandler(string topicPattern, Action<MessagePacket> handler, Communicator communicator)
            => communicator.MessageArrived += (messagePacket) => HandleMessage(topicPattern, messagePacket, handler);
        public void AddMessageHandler(string topicPattern, Action<MessagePacket> handler, IEnumerable<Communicator> communicators)
            => communicators.ForEach(x => AddMessageHandler(topicPattern, handler, x));
    }
}
