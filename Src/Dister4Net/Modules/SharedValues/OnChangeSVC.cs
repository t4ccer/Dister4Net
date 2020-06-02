using System.Collections.Concurrent;
using Dister4Net.Communication;
using Dister4Net.Modules.Communicators;

namespace Dister4Net.Modules.SharedValues
{
    public class OnChangeSVC : SharedValuesController
    {
        public override string ModuleName => "On Change SharedValuesController";
        ConcurrentDictionary<string, object> values;
        Communicator communicator;

        public override T Get<T>(string name)
            => serializer.Deserialize<T>(serializer.Serialize(values[name]));

        public override void Set(string name, object value)
        {
            LocalSet(name, value);
            communicator.SendMessage(new MessagePacket { topic = $"{service.ServiceName}/{service.NodeName}/sharedvalue/{name}", content = serializer.Serialize(value) });
        }
        void LocalSet(string name, object value)
        {
            values[name] = value;
        }
        protected override void Start()
        {
            values = new ConcurrentDictionary<string, object>();
            communicator = service.GetModule<Communicator>();
            AddMessageHandler("+/+/sharedvalue/*", Handler, communicator);
        }
        void Handler(MessagePacket packet)
        {
            LocalSet(packet.SplittedTopic[3], serializer.Deserialize<object>(packet.content));
        }
    }
}
