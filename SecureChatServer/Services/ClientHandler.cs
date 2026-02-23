using System.Net;
using System.Net.Sockets;
using System.Text;
using SecureChatServer.Models;
using SecureChatServer.Models.Packets;

namespace SecureChatServer.Services;

public class ClientHandler(DataHandler dataHandler)
{
    private List<TcpClient> Clients { get; set; } = new List<TcpClient>();
    public Dictionary<TcpClient, string> LoggedInClients { get; set; } = new();// client, name

    public void AddClient(TcpClient client){
        Clients.Add(client);
        _ = Handle(client);
    }
    

// what needed :
// client connects -> get: packet(LOGIN|Name) -> In: check for Login packet | if not -> ignore | -> out: client gets response -> add to chat type shit -> only then he can send mesages      
// clients -> client Handeler -> parsed message -> message handler -> client handler get what to broadcast form message handler

    public bool IsLoggedIn(string name)
    {
        return LoggedInClients.ContainsValue(name);
    }
    
    public string GetName(TcpClient client)
    {
        return LoggedInClients[client];
    }

    private async Task Handle(TcpClient client)
    {

        // IPEndPoint -cast-> endpoint -> .... ->  IPEndPoint
        var remoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint ?? throw new Exception("Client not connected");
        byte[] buffer = new byte[1024];
     
        Console.WriteLine("Client connected from ip: "+remoteEndPoint.Address.ToString());
        while (true)
        {
            int bytes = await client.GetStream().ReadAsync(buffer);
            if (bytes == 0) break; // disconnected

            var packet = buffer.ToPacket(bytes, client);
            if(packet != null)
                _ = dataHandler.HandlePacket(packet);
        }
    }
    
    public async Task BroadcastAllClientsButSenderAsync(string message, TcpClient senderClient)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);

        foreach (var client in LoggedInClients.Keys)
        {
            if (client == senderClient)
                continue;

            try
            {
                await client.GetStream().WriteAsync(data);
            }
            catch
            {
                // ignore dead client
            }
        }
    }
    
    public async Task BroadcastRecieversAsync(string message, List<User> recieverUsers)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);

        foreach (var user in recieverUsers)
        {
            if (LoggedInClients.ContainsValue(user.Username))
            {
                try
                {
                    var client =LoggedInClients.Keys.First(client => LoggedInClients[client] == user.Username);
                    await client.GetStream().WriteAsync(data);
                }
                catch
                {
                    // ignore dead client
                }
            }
        }
    }
    
    
    public async Task BroadcastToClientAsync(string message, TcpClient client)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        
        try
        {
            await client.GetStream().WriteAsync(data);
        }
        catch
        {
            // ignore dead client
        }
    } 
}