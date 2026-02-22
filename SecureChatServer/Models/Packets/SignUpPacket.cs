using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public class SignUpPacket(TcpClient client, string username,string password):Packet
{
    public string Username { get; set; }= username;
    public string Password { get; set; } = password;
    public TcpClient TcpClient { get; set; } = client;
}