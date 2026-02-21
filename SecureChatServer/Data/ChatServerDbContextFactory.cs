using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SecureChatServer.Data;

public class ChatServerDbContextFactory : IDesignTimeDbContextFactory<ChatServerDbContext>
{
    public ChatServerDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            config.GetConnectionString("DefaultConnection")
            ?? throw new Exception("Connection string not found");

        var optionsBuilder =
            new DbContextOptionsBuilder<ChatServerDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new ChatServerDbContext(optionsBuilder.Options);
    }
}