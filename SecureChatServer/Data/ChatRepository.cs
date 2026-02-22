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

    

    public Task<List<Chat>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Chat chat)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Chat chat)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Chat chat)
    {
        throw new NotImplementedException();
    }
}