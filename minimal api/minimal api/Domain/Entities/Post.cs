using dotnet_starter.Domain.Enums;
using dotnet_starter.Domain.ValueObjects;

namespace dotnet_starter.Domain.Entities;

public class Post
{
    public Guid Id { get; private set; }
    public Title? Title { get; private set; }
    public Content? Content { get; private set; }
    public PostStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Foreign key
    public string? UserId { get; private set; }

    // Navigation property
    public virtual User? User { get; private set; }

    private Post()
    {
        // Required by EF Core
    }

    public Post(Title title, Content content, User? user)
    {
        Id = Guid.NewGuid();
        Title = title;
        Content = content;
        Status = PostStatus.Draft;
        User = user;
        if (user != null)
        {
            UserId = user.Id;
        }
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(Title title, Content content)
    {
        Title = title;
        Content = content;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Publish()
    {
        Status = PostStatus.Published;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Archive()
    {
        Status = PostStatus.Archived;
        UpdatedAt = DateTime.UtcNow;
    }
}