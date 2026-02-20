using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public class LoginPacket(TcpClient client, string password,string username) : Packet
{
    public string Password {get; set;}
    public string Username {get; set;}
}