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
    Signup,
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
        if (type == PacketType.Signup)
        {
            SignUpPacket signUpPacket = new SignUpPacket(tcpClient, txtSplit[1], txtSplit[2]);
            return signUpPacket;
        }
        if (type == PacketType.Login)
        {
            LoginPacket loginPacket= new LoginPacket(tcpClient, txtSplit[1], txtSplit[2]);
            return loginPacket;
        }
        if(type== PacketType.Message)
        {
            int recieverid = int.Parse(txtSplit[1]);
            MessagePacket messagePacket= new MessagePacket(tcpClient, recieverid, txtSplit[2]);
            return messagePacket;
        }
        return null;
    }
}