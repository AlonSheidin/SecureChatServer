using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public class MessegePacket(TcpClient client, int recieverId, string msg) : Packet
{
    public int recieverId { get; set; }
    public string msg { get; set; }
}