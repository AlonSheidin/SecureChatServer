using Microsoft.EntityFrameworkCore;
using SecureChatServer.Models;

namespace SecureChatServer.Data;

public class ChatServerDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ----- User -----
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
        });

        // ----- Chat -----
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.ToTable("chats");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired();
        });

        // ----- User <-> Chat Many-to-Many -----
        modelBuilder.Entity<User>()
            .HasMany(u => u.Chats)
            .WithMany(c => c.Users)
            .UsingEntity(j => j.ToTable("users_chats"));

        // ----- Message -----
        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("messages");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Text).IsRequired();
            entity.Property(m => m.SendAt)
                .HasDefaultValueSql("NOW()");

            // Sender relation
            entity.HasOne(m => m.Sender)
                .WithMany()      // User.Messages collection can be added if desired
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Chat relation
            entity.HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}