using Microsoft.EntityFrameworkCore;
using SecureChatServer.Models;

namespace SecureChatServer.Data;

public class ChatRepository(ChatServerDbContext context) : IChatRepository
{
    private ChatServerDbContext _context = context;
    
    public async Task<Chat?> GetByIdAsync(int id)
    {
        return await _context.Chats.
            Include(c => c.Users).
            Include(c => c.Messages).
            FirstOrDefaultAsync(c => c.Id == id);
    }

    

    public async Task<List<Chat>> GetAllAsync()
    {
        return await  _context.Chats.ToListAsync();
    }

    public async Task AddAsync(Chat chat)
    {
         await _context.Chats.AddAsync(chat);
         await  _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Chat chat)
    {
        var chatToChange = await GetByIdAsync(chat.Id);
        if (chatToChange != null)
        {
            chatToChange.Name = chat.Name;
            chatToChange.Users = chat.Users;
            chatToChange.Messages = chat.Messages;
            _context.Chats.Update(chatToChange);
             await _context.SaveChangesAsync();
        }
    }

    public Task DeleteAsync(Chat chat)
    {
        _context.Chats.Remove(chat);
        return _context.SaveChangesAsync();
    }
}