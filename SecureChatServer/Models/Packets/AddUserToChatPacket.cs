using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public class AddUserToChatPacket(TcpClient client, int chatId, string username) : Packet(PacketType.AddUserToChat, client)
{
    public int ChatId {get; set;} = chatId;
    public string Username {get; set;} = username;
}