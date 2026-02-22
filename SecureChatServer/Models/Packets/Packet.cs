namespace SecureChatServer.Models.Packets;

public abstract class Packet (PacketType packetType)
{
    public PacketType Type { get; set; } = packetType;
}

public enum PacketType
{
    Signup,
    Login,
    Message,
    Disconnect,
}