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
    
    public void HandleSendingMessage(MessegePacket messegePacket,User user)
    {
        string username = userRepository.GetByUsernameAsync(user.Username).Result.Username;
        string msgOthers = MessageFormatter.FormatMessageToSender1(MessageType.Self,messegePacket.msg,messegePacket.recieverId,user);
        string msgSelf = MessageFormatter.FormatMessageToSender1(MessageType.Self, messegePacket.msg, messegePacket.recieverId, user);
        ClientHandler.BroadcastToClientAsync(msgSelf, messegePacket.TcpClient);
        ClientHandler.BroadcastRecieversAsync(msgOthers, chatRepository.GetByIdWithUsersAsync());
        
    }

    public void HandleSignUp(MessegePacket messegePacket)
    {
        
    }
}
//ME -> alon | hello
//phil -> you | hello