using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SecureChatServer.Data;

public class ChatServerDbContextFactory : IDesignTimeDbContextFactory<ChatServerDbContext>
{
    public ChatServerDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<ChatServerDbContext>();

        optionBuilder.UseNpgsql(
            ""
        );
        return new ChatServerDbContext(optionBuilder.Options);
    }
}