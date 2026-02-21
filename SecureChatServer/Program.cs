
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SecureChatServer.Data;
using SecureChatServer.Services;

namespace SecureChatServer;

class Program
{
    static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // or Directory.GetCurrentDirectory()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables() // optional: override with env vars
            .Build();

        // Read connection string
        var connectionString = config.GetConnectionString("DefaultConnection")
                               ?? throw new Exception("Connection string not found");
        
        var optionsBuilder = new DbContextOptionsBuilder<ChatServerDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using var context = new ChatServerDbContext(optionsBuilder);
        
        var userRepository = new UserRepository(context);

        var dataHandler = new DataHandler(userRepository);
        var clientHandler = new ClientHandler(dataHandler);

        dataHandler.ClientHandler = clientHandler;

        var server = new TcpServer(5000, clientHandler);
        await server.Start();
    }
}

/*
    src – main source code.

    Models – classes representing data structures.

    Services – business logic.

    Utilities – helpers or extension methods.

    Data – repositories, database access.

    tests – unit and integration tests.
*/

