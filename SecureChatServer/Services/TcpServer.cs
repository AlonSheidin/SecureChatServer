using System.Net;
using System.Net.Sockets;

namespace SecureChatServer.Services;

public class TcpServer(int port, ClientHandler clientHandler)
{
    public int Port { get; set; } = port;

    public async Task Start()
    {
        var listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            clientHandler.AddClient(client);
        }

        listener.Stop();
    }
}


