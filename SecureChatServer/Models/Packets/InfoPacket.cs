using System.Net.Sockets;

namespace SecureChatServer.Models.Packets;

public enum InfoType
{
    MyChats,
    ChatMembers,
    
}
public class InfoPacket(InfoType infoType,TcpClient client,int chatId=-1):Packet(PacketType.Info,client)
{
    public InfoType InfoType{get;set;} = infoType;
    public int ChatId{get;set;} = chatId;
}
//INFO|CHATMEMBERS|1
