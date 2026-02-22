using System.Net.Sockets;
using SecureChatServer.Data;
using SecureChatServer.Models;
using SecureChatServer.Models.Packets;
using SecureChatServer.Utilities;

namespace SecureChatServer.Services;

//TODO IMPLEMENT IChatRepository
public class UserHandler(IUserRepository userRepository, IChatRepository chatRepository)
{
    public ClientHandler ClientHandler {get; set; }
    
    public void HandleSendingMessage(MessegePacket messegePacket)
    {
        string user = ClientHandler.LoggedInClients[messegePacket.TcpClient];
        string msgOthers = MessageFormatter.FormatMessageToSender1(MessageType.Self,messegePacket.msg,messegePacket.recieverId,user);
        string msgSelf = MessageFormatter.FormatMessageToSender1(MessageType.Self, messegePacket.msg, messegePacket.recieverId, user);
        ClientHandler.BroadcastToClientAsync(msgOthers, messegePacket.TcpClient);
        ClientHandler.BroadcastRecieversAsync(msgOthers, chatRepository.GetByIdWithUsersAsync());
        
    }
}
//ME -> alon | hello
//phil -> you | hello