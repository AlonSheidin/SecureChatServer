using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public abstract class Packet (PacketType packetType, TcpClient client)
{
    public TcpClient TcpClient { get; set; } = client;
    public PacketType Type { get; set; } = packetType;
}

public enum PacketType
{
    Signup,
    Login,
    Message,
    Disconnect,
    CreateChat,
    AddUserToChat,
    RemoveUserFromChat
}