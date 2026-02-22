using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace SecureChatServer.Models.Packets;

public class SignUpPacket(TcpClient client, string username,string password) : Packet(PacketType.Signup)
{ 
    public string Username { get; set; }= username;
    public string Password { get; set; } = password;
    public TcpClient TcpClient { get; set; } = client;
}