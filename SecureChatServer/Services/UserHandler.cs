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
    
    public async Task HandleSendingMessageAsync(MessagePacket messagePacket,User user)
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
    
    public async Task CreateChatAsync(CreateChatPacket createChatPacket, User creatorUser)
    {
        var chat = new Chat { Name = createChatPacket.ChatName, Users = new List<User> { creatorUser } };
        await chatRepository.AddAsync(chat);
        creatorUser.Chats.Add(chat);
        await userRepository.UpdateAsync(creatorUser);
        string msg = $"Chat '{createChatPacket.ChatName}' created successfully with ID: {chat.Id}";
        _ = ClientHandler.BroadcastToClientAsync(msg, createChatPacket.TcpClient);
    }
    
    public async Task AddUserToChatAsync(AddUserToChatPacket addUserToChatPacket, User requestingUser)
    {
        var chat = await chatRepository.GetByIdAsync(addUserToChatPacket.ChatId);
        if (chat == null || requestingUser.Chats.All(c => c.Id != chat.Id))
        {
            _ = ClientHandler.BroadcastToClientAsync("You are not a member of this chat.",
                addUserToChatPacket.TcpClient);
            return;
        }
        
        var userToAdd = await userRepository.GetByUsernameAsync(addUserToChatPacket.Username);
        if (userToAdd == null)
        {
            _ = ClientHandler.BroadcastToClientAsync("User not found.", addUserToChatPacket.TcpClient);
            return;
        }
        
        if (userToAdd.Id == requestingUser.Id)
        {
            _ = ClientHandler.BroadcastToClientAsync("Cannot add yourself to the chat.", addUserToChatPacket.TcpClient);
            return;
        }

        if (chat.Users.Contains(userToAdd))
        {
            _ = ClientHandler.BroadcastToClientAsync("User is already a member of the chat.", addUserToChatPacket.TcpClient);
            return;
        }
        

        chat.Users.Add(userToAdd);
        await chatRepository.UpdateAsync(chat);
        
        userToAdd.Chats.Add(chat);
        await userRepository.UpdateAsync(userToAdd);
    
        _ = ClientHandler.BroadcastToClientAsync($"User '{addUserToChatPacket.Username}' added to chat '{chat.Name}' successfully.", addUserToChatPacket.TcpClient);
    }
    
    public async Task RemoveUserFromChatAsync(RemoveUserFromChatPacket removeUserFromChatPacket, User requestingUser)
    {
        var chat = await chatRepository.GetByIdAsync(removeUserFromChatPacket.ChatId);
        if (chat == null || requestingUser.Chats.All(c => c.Id != chat.Id))
        {
            _ = ClientHandler.BroadcastToClientAsync("You are not a member of this chat.",
                removeUserFromChatPacket.TcpClient);
            return;
        }
        
        var userToRemove = await userRepository.GetByUsernameAsync(removeUserFromChatPacket.Username);
        if (userToRemove == null)
        {
            _ = ClientHandler.BroadcastToClientAsync("User not found.", removeUserFromChatPacket.TcpClient);
            return;
        }
        
        if (!chat.Users.Contains(userToRemove))
        {
            _ = ClientHandler.BroadcastToClientAsync("User is not a member of the chat.", removeUserFromChatPacket.TcpClient);
            return;
        }

        chat.Users.Remove(userToRemove);
        await chatRepository.UpdateAsync(chat);
        
        userToRemove.Chats.Remove(chat);
        await userRepository.UpdateAsync(userToRemove);
    
        _ = ClientHandler.BroadcastToClientAsync($"User '{removeUserFromChatPacket.Username}' removed from chat '{chat.Name}' successfully.", removeUserFromChatPacket.TcpClient);
    }

    public async Task SendUserChats(List<Chat> chats, TcpClient client)
    {
        foreach (var chat in chats)
        {
            _ = ClientHandler.BroadcastToClientAsync($"Chat Name:{chat.Name} Id:{chat.Id}",client);
        }
    }

    public async Task SendChatUsers(Chat chat,TcpClient client)
    {
        foreach (var user in chat.Users)
        {
            _ = ClientHandler.BroadcastToClientAsync($"{user.Username}",client);
        }
    }
}
//ME -> alon | hello
//phil -> you | hello