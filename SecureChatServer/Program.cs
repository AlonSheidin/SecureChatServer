
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
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        // Read connection string
        var connectionString = config.GetConnectionString("DefaultConnection")
                               ?? throw new Exception("Connection string not found");
        
        var optionsBuilder = new DbContextOptionsBuilder<ChatServerDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using var context = new ChatServerDbContext(optionsBuilder);
        
        var userRepository = new UserRepository(context);
        var chatRepository = new ChatRepository(context);
        
        var dataHandler = new DataHandler(userRepository, chatRepository);
        var clientHandler = new ClientHandler(dataHandler);
        var userHandler = new UserHandler(userRepository, chatRepository)
        {
            ClientHandler = clientHandler
        };
        
        dataHandler.ClientHandler = clientHandler;
        dataHandler.UserHandler = userHandler;
        
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

