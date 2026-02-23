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
    
    public async Task HandleSendingMessage(MessagePacket messagePacket,User user)
    {
        string? sender = (await userRepository.GetByUsernameAsync(user.Username))?.Username;
        string msgSelf = MessageFormatter.FormatMessageToSender1(MessageType.Self,messagePacket.Message,messagePacket.ReceiverId,sender);
        string msgOthers = MessageFormatter.FormatMessageToSender1(MessageType.Others, messagePacket.Message, messagePacket.ReceiverId, sender);
        
        _ =ClientHandler.BroadcastToClientAsync(msgSelf, messagePacket.TcpClient);
        var chatToSend = await chatRepository.GetByIdAsync(messagePacket.ReceiverId);
        if (chatToSend != null && chatToSend.Users.Count > 0)
        {
            var usersToSend = chatToSend.Users
                .Where(u => u.Id != user.Id).ToList();
            
            _ =ClientHandler.BroadcastRecieversAsync(msgOthers, usersToSend);
        }
    }

    public async Task HandleSignUp(SignUpPacket signUpPacket)
    {
        if (await userRepository.GetByUsernameAsync(signUpPacket.Username) != null)
        {
            _ = ClientHandler.BroadcastToClientAsync("Sign Up failed, Username already in user", signUpPacket.TcpClient);
            return;
        }
        User user = new User{Username =  signUpPacket.Username,PasswordHash = PasswordHelper.HashPassword(signUpPacket.Password)};
        await userRepository.AddAsync(user);
        _ =ClientHandler.BroadcastToClientAsync("Sign Up successful", signUpPacket.TcpClient);
        
    }
    //SIGNUP|PHIL|HELLO
}
//ME -> alon | hello
//phil -> you | hello