using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public class MessagePacket(TcpClient client, int receiverId, string message) : Packet(PacketType.Message, client)
{
    public int ReceiverId { get; set; } = receiverId;
    public string Message { get; set; } = message;
}