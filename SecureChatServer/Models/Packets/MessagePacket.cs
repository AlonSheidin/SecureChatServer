using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public class MessagePacket(TcpClient client, int recieverId, string msg) : Packet
{
    public TcpClient TcpClient { get; set; }
    public int recieverId { get; set; }
    public string msg { get; set; }
}