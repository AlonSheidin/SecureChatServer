using SecureChatServer.Models;

namespace SecureChatServer.Data;

public interface IChatRepository
{
    Task<Chat?> GetByIdAsync(int id);
    
    Task<List<Chat>> GetAllAsync();
    Task AddAsync(Chat chat);
    Task UpdateAsync(Chat chat);
    Task DeleteAsync(Chat chat);
}