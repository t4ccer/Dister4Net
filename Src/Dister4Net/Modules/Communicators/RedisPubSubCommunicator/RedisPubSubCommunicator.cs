using System;
using System.Collections.Generic;
using System.Text;
using Dister4Net.Attributes;
using Dister4Net.Communication;
using Dister4Net.Modules.Connectors;
using StackExchange.Redis;

namespace Dister4Net.Modules.Communicators.RedisPubSubCommunicator
{
    [RequiresModule(typeof(RedisConnector))]
    public class RedisPubSubCommunicator : Communicator
    {
        public override string ModuleName => "Redis Pub Sub Communicator";
        ISubscriber subscriber;

        public override void SendMessage(MessagePacket message)
        {
            subscriber.PublishAsync("Dister", serializer.Serialize(message));
            OnMessageSent("Redis Pub Sub Communicator", message);
        }
        protected override void Start()
        {
            subscriber = service.GetModule<RedisConnector>().GetSubscriber();
            subscriber.Subscribe("Dister", (channel, message) => 
            {
                var packet = serializer.Deserialize<MessagePacket>(Encoding.Default.GetString((byte[])message.Box()));
                OnMessageArrived(packet);
            });
        }
    }
}
