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
        string? username = (await userRepository.GetByUsernameAsync(user.Username))?.Username;
        string msgOthers = MessageFormatter.FormatMessageToSender1(MessageType.Self,messagePacket.msg,messagePacket.recieverId,username);
        string msgSelf = MessageFormatter.FormatMessageToSender1(MessageType.Self, messagePacket.msg, messagePacket.recieverId, username);
        _ =ClientHandler.BroadcastToClientAsync(msgSelf, messagePacket.TcpClient);
        _ =ClientHandler.BroadcastRecieversAsync(msgOthers, (await chatRepository.GetByIdAsync(messagePacket.recieverId)).Users);
        
    }

    public async Task HandleSignUp(SignUpPacket signUpPacket)
    {
        if (await userRepository.GetByUsernameAsync(signUpPacket.username)==null)
        {
            _ =ClientHandler.BroadcastToClientAsync("Sign Up failed", signUpPacket.tcpClient);
        }
        User user = new User{Username =  signUpPacket.username,PasswordHash = PasswordHelper.HashPassword(signUpPacket.password)};
        await userRepository.AddAsync(user);
        _ =ClientHandler.BroadcastToClientAsync("Sign Up successful", signUpPacket.tcpClient);
        
    }
    //SIGNUP|PHIL|HELLO
}
//ME -> alon | hello
//phil -> you | hello