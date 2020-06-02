using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Dister4Net.Communication;

namespace Dister4Net.Modules.Communicators.SocketCommunicator
{
    public class MasterSocketCommunicator : Communicator
    {
        public override string ModuleName => "Master Socket Communicator";

        private Socket listener;
        private readonly ConcurrentDictionary<string, Socket> connections = new ConcurrentDictionary<string, Socket>();
        private readonly int port = 1337;
        private readonly bool redistributeMessages = false;

        public MasterSocketCommunicator()
        {

        }
        public MasterSocketCommunicator(int port)
        {
            this.port = port;
        }
        public MasterSocketCommunicator(bool redistributeMessages)
        {
            this.redistributeMessages = redistributeMessages;
        }
        public MasterSocketCommunicator(bool redistributeMessages, int port)
        {
            this.redistributeMessages = redistributeMessages;
            this.port = port;
        }


        protected override void Start()
        {
            CreateListener();
            StartAcceptor();
        }
        private void StartAcceptor()
        {
            new Thread(() =>
            {
                while (true)
                {
                    var newSocket = listener.Accept();
                    var packet = newSocket.ReceiveMessagePacket(serializer);
                    var workerName = packet.SplittedTopic[1];
                    new Thread(() => ReceiveFromWorker(workerName, newSocket)) { Name = workerName + " Receiver" }.Start();
                    connections.TryAdd(workerName, newSocket);
                    Console.WriteLine($"New worker: {workerName}");
                }
            })
            { Name = "Acceptor" }.Start();
        }

        private void ReceiveFromWorker(string workerName, Socket socket)
        {
            while (socket.IsOpen())
            {
                try
                {
                    var message = socket.ReceiveMessagePacket(serializer);
                    if (redistributeMessages)
                        SendMessage(message);
                    OnMessageArrived(message);
                }
                catch (SocketException)
                {
                    break;
                }
            }
            connections.TryRemove(workerName, out var _);
            Console.WriteLine($"Removed worker: {workerName}");
        }
        private void CreateListener()
        {
            var endpoint = new IPEndPoint(IPAddress.Any, port);
            listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(endpoint);
            listener.Listen(2048);
        }
        public override void SendMessage(MessagePacket message)
        {
            foreach (var worker in connections)
            {
                OnMessageSent(worker.Key, message);
                worker.Value.Send(message.SerializeForSocket(serializer));
            }
        }
    }
}
