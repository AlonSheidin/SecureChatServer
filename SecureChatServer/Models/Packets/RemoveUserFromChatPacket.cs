using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public class RemoveUserFromChatPacket(TcpClient client, int chatId, string userName) : Packet(PacketType.RemoveUserFromChat, client)
{
    public int ChatId { get; set; } = chatId;
    public string UserName { get; set; } = userName;
}