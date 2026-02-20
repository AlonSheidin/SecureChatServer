using SecureChatServer.Models.Packets;

namespace SecureChatServer.Utilities;

public static class MessageFormatter
{
    public static string FormatMessageToAllClients(PacketType type,  string message, string name)
    {
        return type switch
        {
            PacketType.Login => $"Login | {name}",
            PacketType.Message => $"{name} | {message}",
            PacketType.Disconnect => $"Disconnect | {message}"
        };
    }

    public static string FormatMessageToSender(PacketType type, string message, string name)
    {
        return type switch
        {
            PacketType.Login => $"Logged as {name} successfully",
            PacketType.Message => $"Me | {message}",
            PacketType.Disconnect => $"Disconnect as {name} successfully"
        };
    }
}
