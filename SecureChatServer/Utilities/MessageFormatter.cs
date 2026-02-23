using System.Diagnostics;
using SecureChatServer.Models.Packets;

namespace SecureChatServer.Utilities;

public static class MessageFormatter
{
    public static string FormatMessageToOtherClients(PacketType type,  string message, string name)
    {
        return type switch
        {
            PacketType.Login => $"Login | {name}",
            PacketType.Message => $"{name} | {message}",
            PacketType.SignOut => $"Disconnect | {message}"
        };
    }

    public static string FormatMessageToSender(PacketType type, string message, string name)
    {
        return type switch
        {
            PacketType.Login => $"Logged as {name} successfully",
            PacketType.Message => $"Me | {message}",
            PacketType.SignOut => $"Disconnect as {name} successfully"
        };
    }

    public static string FormatMessageToSender1(MessageType messageType, string message, int reciever, string name)
    {
        return messageType switch
        {
            MessageType.Self => $"Me -> {reciever} | {message}",
            MessageType.Others => $"{name} -> {reciever} | {message}"
        };
    }
}
public enum MessageType
{
    Self,
    Others,
}
