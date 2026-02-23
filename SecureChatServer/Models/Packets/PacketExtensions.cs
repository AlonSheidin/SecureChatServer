using System.Net.Sockets;
using System.Text;

namespace SecureChatServer.Models.Packets;

public static class PacketExtensions
{
    public static Packet? ToPacket(this byte[] buffer, int bytes,TcpClient tcpClient)
    {
        string txt = Encoding.UTF8.GetString(buffer, 0, bytes);
        string[] txtSplit = txt.Split('|');
        //"hello|world|ass" -> string[] = {hello, world, a}
        bool canParse = Enum.TryParse<PacketType>(txtSplit[0], ignoreCase:true, out var type);
        if(!canParse)
            return null;
        switch (type)
        {
            case PacketType.Signup:
            {
                SignUpPacket signUpPacket = new SignUpPacket(tcpClient, txtSplit[1], txtSplit[2]);
                return signUpPacket;
            }
            case PacketType.Login:
            {
                LoginPacket loginPacket = new LoginPacket(tcpClient, txtSplit[1], txtSplit[2]);
                return loginPacket;
            }
            case PacketType.Message:
            {
                int receiverId = int.Parse(txtSplit[1]);
                var messagePacket = new MessagePacket(tcpClient, receiverId, txtSplit[2]);
                return messagePacket;
            }
            case PacketType.CreateChat:
            {
                var createChatPacket = new CreateChatPacket(tcpClient, txtSplit[1]);
                return createChatPacket;
            }
            case PacketType.AddUserToChat:
            {
                var addUserToChatPacket = new AddUserToChatPacket(tcpClient, int.Parse(txtSplit[1]), txtSplit[2]);
                return  addUserToChatPacket;
            }
            case PacketType.RemoveUserFromChat:
            {
                var removeUserFromChatPacket = new RemoveUserFromChatPacket(tcpClient, int.Parse(txtSplit[1]), txtSplit[2]);
                return removeUserFromChatPacket;
            }
            default:
                return null;
        }
    }
}