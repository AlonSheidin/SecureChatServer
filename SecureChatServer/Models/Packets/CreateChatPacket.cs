using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public class CreateChatPacket(TcpClient client ,string chatName) : Packet(PacketType.CreateChat, client)
{
    public string ChatName {get; set; } = chatName;
}