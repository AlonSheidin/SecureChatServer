namespace SecureChatServer.Models.Packets;
using System.Net.Sockets;

public class SignOutPacket(TcpClient client):Packet(PacketType.SignOut,client)
{
    public TcpClient Client { get; set; }= client;
}