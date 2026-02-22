using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public class SignUpPacket(TcpClient client, string username,string password):Packet
{
    public string username;
    public string password;
    public TcpClient tcpClient{get;}
}