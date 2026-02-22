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
        string msgOthers = MessageFormatter.FormatMessageToSender1(MessageType.Self,messagePacket.Message,messagePacket.ReceiverId,username);
        string msgSelf = MessageFormatter.FormatMessageToSender1(MessageType.Self, messagePacket.Message, messagePacket.ReceiverId, username);
        _ =ClientHandler.BroadcastToClientAsync(msgSelf, messagePacket.TcpClient);
        _ =ClientHandler.BroadcastRecieversAsync(msgOthers, (await chatRepository.GetByIdAsync(messagePacket.ReceiverId)).Users);
        
    }

    public async Task HandleSignUp(SignUpPacket signUpPacket)
    {
        if (await userRepository.GetByUsernameAsync(signUpPacket.Username)==null)
        {
            _ =ClientHandler.BroadcastToClientAsync("Sign Up failed", signUpPacket.TcpClient);
        }
        User user = new User{Username =  signUpPacket.Username,PasswordHash = PasswordHelper.HashPassword(signUpPacket.Password)};
        await userRepository.AddAsync(user);
        _ =ClientHandler.BroadcastToClientAsync("Sign Up successful", signUpPacket.TcpClient);
        
    }
    //SIGNUP|PHIL|HELLO
}
//ME -> alon | hello
//phil -> you | hello