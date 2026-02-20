namespace SecureChatServer.Models;

public class Message
{
    public int Id { get; set; }
    public string Text { get; set; } = "";

    // Foreign key to User
    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;

    // Foreign key to Chat
    public int ChatId { get; set; }
    public Chat Chat { get; set; } = null!;

    public DateTime SendAt { get; set; }
}