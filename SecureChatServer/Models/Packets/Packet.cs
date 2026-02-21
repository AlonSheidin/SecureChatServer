using System.Net.Sockets;
using System.Text;

namespace SecureChatServer.Models.Packets;

public abstract class Packet
{
    public TcpClient Client { get; set; }
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
    public static Packet? ToPacket(this byte[] buffer, int bytes,TcpClient tcpClient)
    {
        string txt = Encoding.UTF8.GetString(buffer, 0, bytes);
        string[] txtSplit = txt.Split('|');
        //"hello|world|ass" -> string[] = {hello, world, a}
        Enum.TryParse<PacketType>(txtSplit[0], out PacketType type);
        if (type == PacketType.Login)
        {
            LoginPacket loginPacket= new LoginPacket(tcpClient, txtSplit[1], txtSplit[2]);
            return loginPacket;
        }
        if(type== PacketType.Message)
        {
            int recieverid = int.Parse(txtSplit[1]);
            MessegePacket messegePacket= new MessegePacket(tcpClient, recieverid, txtSplit[2]);
            return messegePacket;
        }
        return null;
    }
}