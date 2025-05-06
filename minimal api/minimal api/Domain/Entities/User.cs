using dotnet_starter.Domain.Enums;
using dotnet_starter.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace dotnet_starter.Domain.Entities;

public class User : IdentityUser
{
    public Name? Name { get; private set; }
    public UserStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    // Navigation property
    public virtual ICollection<Post> Posts { get; private set; } = new List<Post>();

    public User()
    {
        // Required by EF Core
    }

    public User(string userName, string email, Name? name)
    {
        UserName = userName;
        Email = email;
        Name = name;
        Status = UserStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(Name? name, UserStatus status)
    {
        Name = name;
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = UserStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }
}