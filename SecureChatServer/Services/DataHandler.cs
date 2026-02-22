using System.Net.Sockets;
using SecureChatServer.Data;
using SecureChatServer.Models;
using SecureChatServer.Models.Packets;
using SecureChatServer.Utilities;

namespace SecureChatServer.Services;

public class DataHandler( IUserRepository userRepository)
{
    public UserHandler UserHandler { get; set; }
    
    
    public ClientHandler ClientHandler {get; set; }


    public async Task HandlePacket(Packet packet, TcpClient tcpClient)
    {
        switch (packet.Type)
        {
            case PacketType.Signup:
                SignUpPacket? signUpPacket = packet as SignUpPacket?? throw new Exception("Signup packet is null");
                await UserHandler.HandleSignUp(signUpPacket);
                break;

            case PacketType.Login:
                LoginPacket? loginPacket = packet as LoginPacket;

                // HANDLE CLIENT LOGIN (AUTHENTICATING PASSWORD AND USERNAME)

                var user = await userRepository.GetByUsernameAsync(loginPacket.Username);
                if (user != null && PasswordHelper.VerifyPassword(loginPacket.Password, user.PasswordHash))
                {
                    ClientHandler.LoggedInClients.Add(tcpClient, loginPacket.Username);
                }
            
                break;


            case PacketType.Message:
                MessagePacket? messegePacket = packet as MessagePacket;
                if (ClientHandler.LoggedInClients.ContainsKey(tcpClient))
                {
                    var user1 = await userRepository.GetByUsernameAsync(ClientHandler.LoggedInClients[tcpClient]);
                    if (user1 != null&& messegePacket?.Message != null)
                    {
                        ClientHandler.LoggedInClients.Add(tcpClient, user1.Username);
                        await UserHandler.HandleSendingMessage(messegePacket,user1);
                    }
                }
                // HANDLE CLIENT MESSAGE -> TO USER HANDLER
                break;

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