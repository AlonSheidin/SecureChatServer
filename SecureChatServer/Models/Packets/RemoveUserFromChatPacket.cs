using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public class RemoveUserFromChatPacket(TcpClient client, int chatId, string username) : Packet(PacketType.RemoveUserFromChat, client)
{
    public int ChatId { get; set; } = chatId;
    public string Username { get; set; } = username;
}