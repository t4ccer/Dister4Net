using System;
using System.Collections.Generic;
using System.Text;
using Dister4Net.Communication;
using Dister4Net.Exceptions;
using Dister4Net.Modules.Communicators;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Dister4Net.Modules.Communicators.MqttCommunicator
{
    public class MqttCommunicator : Communicator
    {
        public override string ModuleName => "MQTT Communicator";
        MqttClient client;
        public MqttCommunicator(string hostname)
        {
            client = new MqttClient(hostname);
        }

        public override void SendMessage(MessagePacket message)
        {
            client.Publish(message.topic, message.content != null ? Encoding.UTF8.GetBytes(message.content) : null);
            OnMessageSent(message.SplittedTopic[1], message);
        }
        protected override void Start()
        {
            try
            {
                client.Connect(service.NodeName);
            }
            catch (Exception ex)
            {
                throw new CommunicatorException("Connection to mqtt broker failed", ex);
            }
            client.Subscribe(new[] { "#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
        }

        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var content = Encoding.UTF8.GetString(e.Message);
            var packet = new MessagePacket { content = content, topic = e.Topic };
            //if (packet.SplittedTopic[0] != service.ServiceName && packet.SplittedTopic[1] != service.NodeName)
            OnMessageArrived(packet);
        }
    }
}
