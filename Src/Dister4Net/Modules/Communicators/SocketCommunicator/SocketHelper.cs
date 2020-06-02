using System.Net.Sockets;
using System.Text;
using Dister4Net.Communication;
using Dister4Net.Serialization;

namespace Dister4Net.Modules.Communicators.SocketCommunicator
{
    internal static class SocketHelper
    {
        internal static void Send(this Socket socket, string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            socket.Send(buffer);
        }
        internal static int ReceivePacketLength(this Socket socket)
        {
            var buffer = new byte[8];
            socket.Receive(buffer);
            var str = Encoding.UTF8.GetString(buffer);
            return int.Parse(str, System.Globalization.NumberStyles.HexNumber);
        }
        internal static MessagePacket ReceiveMessagePacket(this Socket socket, ISerializer serializer)
        {
            var length = socket.ReceivePacketLength();
            var buffer = new byte[length];
            socket.Receive(buffer);
            return serializer.Deserialize<MessagePacket>(Encoding.UTF8.GetString(buffer));
        }
        internal static string SerializeForSocket(this MessagePacket messagePacket, ISerializer serializer)
        {
            var serialized = serializer.Serialize(messagePacket);
            var length = serialized.Length.ToString("x8");
            return length + serialized;
        }
        internal static bool IsOpen(this Socket s)
             => !(s.Poll(1000, SelectMode.SelectRead) && s.Available == 0 || !s.Connected);
    }
}
