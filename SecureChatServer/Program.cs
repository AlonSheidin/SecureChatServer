
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SecureChatServer.Data;
using SecureChatServer.Services;

namespace SecureChatServer;

class Program
{
    static async Task Main()
    {
        Env.Load();// loads .env into Environment variables
        Console.WriteLine("env: "+Environment.GetEnvironmentVariable("CHAT_DB"));
        var connectionString = Environment.GetEnvironmentVariable("CHAT_DB")
                               ?? throw new Exception("DB connection string not set");
        /*var options = new DbContextOptionsBuilder<ChatServerDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using var context = new ChatServerDbContext(options);
        
        var userRepository = new UserRepository(context);

        var dataHandler = new DataHandler(userRepository);
        var clientHandler = new ClientHandler(dataHandler);

        dataHandler.ClientHandler = clientHandler;

        var server = new TcpServer(5000, clientHandler);
        await server.Start();*/
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

