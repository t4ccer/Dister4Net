using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dister4Net.Communication;
using Dister4Net.Exceptions;
using Dister4Net.Modules.Communicators;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dister4Net.Modules.Communicators.AMQPCommunicator
{
    public class AMQPCommunicator : Communicator
    {
        public override string ModuleName => "AMQP Communicator";
        string hostname, username, password;
        int port;
        IConnection connection;
        IModel channel;

        public AMQPCommunicator(string hostname, int port, string username, string password)
        {
            this.hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));
            this.port = port;
            this.username = username ?? throw new ArgumentNullException(nameof(username));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public override void SendMessage(MessagePacket message)
        {
            var topic = message.topic.Replace('*', '#').Replace('+', '*').Replace('/', '.');
            var body = message.content == null ? Encoding.UTF8.GetBytes("") : Encoding.UTF8.GetBytes(message.content);
            channel.BasicPublish(exchange: "main", routingKey: topic, basicProperties: null, body: body);
            OnMessageSent(message.SplittedTopic[1], message);
        }
        void Receive()
        {
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "main", routingKey: "#");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var content = Encoding.UTF8.GetString(body);
                var topic = ea.RoutingKey.Replace('.', '/').Replace('*', '+').Replace('#', '*');
                var packet = new MessagePacket { content = content, topic = topic };
                OnMessageArrived(packet);
            };
            channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);
        }
        protected override void Start()
        {
            var factory = new ConnectionFactory { HostName = hostname, UserName = username, Password = password, Port = port };
            try
            {
                connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                throw new CommunicatorException("Connection to AMQP server failed", ex);
            }
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare("main", "topic");
            SendMessage(new MessagePacket { topic = $"{service.ServiceName}/{service.NodeName}/welcome" });
            Receive();
        }

    }
}
