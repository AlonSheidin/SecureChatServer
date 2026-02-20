namespace SecureChatServer.Models;

public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Message> Messages { get; set; } = new();
    public List<User> Users { get; set; } = new();
}