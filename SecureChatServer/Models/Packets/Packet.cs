using System.Net.Sockets;
using System.Text;

namespace SecureChatServer.Models.Packets;

public abstract class Packet
{
    public required TcpClient Client { get; set; }
    public PacketType Type { get; set; }
}

public enum PacketType
{
    Login,
    Message,
    Disconnect,
}

public static class PacketExtensions
{
    public static Packet? ToPacket(this byte[] buffer, int bytes)
    {
        string txt = Encoding.UTF8.GetString(buffer, 0, bytes);
        string[] txtSplit = txt.Split('|');
        //"hello|world|ass" -> string[] = {hello, world, ass}
        Enum.TryParse<PacketType>(txtSplit[0], out PacketType type);
        if (type == PacketType.Login)
        {
            ;
        }
        if(type== PacketType.Message)
        {
            ;
        }
        return null;
    }
}