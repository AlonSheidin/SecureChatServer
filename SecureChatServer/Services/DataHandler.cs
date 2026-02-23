using System.Net.Sockets;
using SecureChatServer.Data;
using SecureChatServer.Models;
using SecureChatServer.Models.Packets;
using SecureChatServer.Utilities;

namespace SecureChatServer.Services;

public class DataHandler( IUserRepository userRepository,IChatRepository chatRepository)
{
    public UserHandler UserHandler { get; set; }
    
    
    public ClientHandler ClientHandler {get; set; }


    public async Task HandlePacket(Packet packet)
    {
        switch (packet.Type)
        {
            case PacketType.SignOut:
                var  signOutPacket = packet as SignOutPacket ?? throw new Exception("SignOut packet is null");
                if (ClientHandler.LoggedInClients.Remove(signOutPacket.TcpClient))
                {
                    _=ClientHandler.BroadcastToClientAsync("Signout successful",signOutPacket.TcpClient);
                }
                else
                    _ =ClientHandler.BroadcastToClientAsync("User not logged in",signOutPacket.TcpClient);
                break;
            
            case PacketType.Signup:
                var signUpPacket = packet as SignUpPacket?? throw new Exception("Signup packet is null");
                await UserHandler.HandleSignUp(signUpPacket);
                ClientHandler.BroadcastToClientAsync($"Signed up as {signUpPacket.Username}",signUpPacket.TcpClient);
                break;

            case PacketType.Login:
                var loginPacket = packet as LoginPacket ?? throw new Exception("Login packet is null");
                var user = await userRepository.GetByUsernameAsync(loginPacket.Username);
                if (user != null && PasswordHelper.VerifyPassword(loginPacket.Password, user.PasswordHash))
                {
                    ClientHandler.LoggedInClients.Add(packet.TcpClient, user.Username);
                    ClientHandler.BroadcastToClientAsync($"Logged in as {loginPacket.Username}",loginPacket.TcpClient);
                }
            
                break;


            case PacketType.Message:
                var messagePacket = packet as MessagePacket ?? throw new Exception("Message packet is null");
                if (ClientHandler.LoggedInClients.TryGetValue(packet.TcpClient, out var usernameMessage))
                {
                    var messageSender = await userRepository.GetByUsernameAsync(usernameMessage);
                    if (messageSender != null && messagePacket?.Message != null)
                    {
                        await UserHandler.HandleSendingMessageAsync(messagePacket,messageSender);
                    }
                }
                // HANDLE CLIENT MESSAGE -> TO USER HANDLER
                break;
            
            case PacketType.CreateChat:
            var createChatPacket = packet as CreateChatPacket ?? throw new Exception("CreateChat packet is null");
            if (ClientHandler.LoggedInClients.TryGetValue(packet.TcpClient, out var usernameCreateChat))
            {
                
                var creatorUser = await userRepository.GetByUsernameAsync(usernameCreateChat);
                if (creatorUser != null) 
                    await UserHandler.CreateChatAsync(createChatPacket, creatorUser);
            }
            break;
            case PacketType.Info:
                var infoPacket = packet as InfoPacket ?? throw new Exception("Info packet is null");
                switch (infoPacket.InfoType)
                {
                    case InfoType.MyChats:
                        var username = ClientHandler.LoggedInClients[infoPacket.TcpClient];
                        var user1 = await userRepository.GetByUsernameAsync(username);
                        List<Chat> chats= user1.Chats;
                        await UserHandler.SendUserChats(chats,infoPacket.TcpClient);
                        break;
                    case InfoType.ChatMembers:
                        var chat = await chatRepository.GetByIdAsync(infoPacket.ChatId);
                        await UserHandler.SendChatUsers(chat, infoPacket.TcpClient);
                        break;
                }break;
        }
    }

    /*public void HandlePacket(Packet? packet, TcpClient client)
    {
        var message = "";//packet.Message;
        switch (packet.Type)
        {
            //
            case PacketType.Login:
                if (!ClientHandler.IsLoggedIn(message))
                {
                    ClientHandler.LoggedInClients.Add(client, message);
                    _ = ClientHandler.BroadcastToClientAsync(
                        MessageFormatter.FormatMessageToSender(PacketType.Login, "", message), client);
                    _ = ClientHandler.BroadcastAllClientsButSenderAsync(
                        MessageFormatter.FormatMessageToOtherClients(PacketType.Login, "", message), client);
                }
                else
                {
                    _ = ClientHandler.BroadcastToClientAsync($"Unable to log in as {message}", client);
                }

                break;
            case PacketType.Disconnect:
                if (ClientHandler.IsLoggedIn(message))
                {
                    _ = ClientHandler.LoggedInClients.Remove(client);
                }

                break;

            case PacketType.Message:
                var name = ClientHandler.GetName(client);
                if (ClientHandler.IsLoggedIn(name))
                {
                    _ = ClientHandler.BroadcastToClientAsync(
                        MessageFormatter.FormatMessageToSender(PacketType.Message, message, name), client);
                    _ = ClientHandler.BroadcastAllClientsButSenderAsync(
                        MessageFormatter.FormatMessageToOtherClients(PacketType.Message, message, name), client);
                }

                break;
        }
    }*/
}