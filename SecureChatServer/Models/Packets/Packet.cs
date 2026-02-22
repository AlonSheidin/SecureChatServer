namespace SecureChatServer.Models.Packets;

public abstract class Packet
{
    public PacketType Type { get; set; }
}

public enum PacketType
{
    Signup,
    Login,
    Message,
    Disconnect,
}