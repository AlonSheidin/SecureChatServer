using SecureChatServer.Models;

namespace SecureChatServer.Data;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByIdWithChatsAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    
    Task<User?> GetByUsernameWithChatsAsync(string username);
    Task<List<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}