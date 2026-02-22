using Microsoft.EntityFrameworkCore;
using SecureChatServer.Models;

namespace SecureChatServer.Data;

public class UserRepository(ChatServerDbContext context) : IUserRepository
{

    private ChatServerDbContext _context = context;

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.
            FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<User?> GetByIdWithChatsAsync(int id)
    {
        return await _context.Users.
            Include(u => u.Chats).
            FirstOrDefaultAsync(u => u.Id == id);
    }



    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByUsernameWithChatsAsync(string username)
    {
        return await _context.Users.
                Include(u => u.Chats).
                FirstOrDefaultAsync(u => u.Username == username);
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        var userToChange = await GetByIdWithChatsAsync(user.Id);
        if (userToChange != null)
        {
            userToChange.Username = user.Username;
            userToChange.PasswordHash = user.PasswordHash;
            userToChange.Chats = user.Chats;
            _context.Users.Update(userToChange);
             await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}