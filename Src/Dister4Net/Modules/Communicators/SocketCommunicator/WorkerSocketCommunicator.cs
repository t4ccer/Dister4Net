using System;
using System.Net.Sockets;
using System.Threading;
using Dister4Net.Communication;
using Dister4Net.Exceptions;

namespace Dister4Net.Modules.Communicators.SocketCommunicator
{
    public class WorkerSocketCommunicator : Communicator
    {
        public override string ModuleName => "Worker Socket Communicator";

        private readonly Socket socket;
        private readonly string host;
        private readonly int port = 1337;

        public WorkerSocketCommunicator(string host)
        {
            this.host = host;
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }
        public WorkerSocketCommunicator(string host, int port) : this(host)
        {
            this.port = port;
        }


        public override void SendMessage(MessagePacket message)
        {
            socket.Send(message.SerializeForSocket(serializer));
            OnMessageSent("master", message);
        }
        protected override void Start()
        {
            try
            {
                socket.Connect(host, port);
            }
            catch (Exception ex)
            {
                throw new CommunicatorException("Connection to master failed", ex);
            }
            SendMessage(new MessagePacket { topic = $"{service.ServiceName}/{service.NodeName}/Welcome" });
            new Thread(() => ReceiveFromMaster()) { Name = "Receiver" }.Start();
        }
        private void ReceiveFromMaster()
        {
            while (socket.IsOpen())
            {
                try
                {
                    var message = socket.ReceiveMessagePacket(serializer);
                    OnMessageArrived(message);
                }
                catch (SocketException)
                {
                    break;
                }
            }
            throw new CommunicatorException("Master closed connection");
        }
    }
}
