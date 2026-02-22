using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public class LoginPacket(TcpClient client,string username, string password) : Packet(PacketType.Login)
{
    public string Password {get; set;} = password;
    public string Username {get; set;} = username;
}