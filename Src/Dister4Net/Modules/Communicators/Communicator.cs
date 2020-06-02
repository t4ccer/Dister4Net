using System;
using System.Collections.Generic;
using System.Text;
using Dister4Net.Communication;
using Dister4Net.Modules;

namespace Dister4Net.Modules.Communicators
{
    public abstract class Communicator : Module
    {
        public delegate void MessageArrivedHandler(MessagePacket message);
        public event MessageArrivedHandler MessageArrived;
        public void OnMessageArrived(MessagePacket message) => MessageArrived?.Invoke(message);


        public delegate void MessageSentHandler(string target, MessagePacket message);
        public event MessageSentHandler MessageSent;
        public void OnMessageSent(string target, MessagePacket message) => MessageSent?.Invoke(target, message);

        public abstract void SendMessage(MessagePacket message);
    }
}
