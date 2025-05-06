using dotnet_starter.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dotnet_starter.Infrastructure;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            // Configure Name value object
            entity.OwnsOne(u => u.Name, name =>
            {
                name.Property(n => n.FirstName).HasColumnName("FirstName").IsRequired();
                name.Property(n => n.LastName).HasColumnName("LastName").IsRequired();
            });

            // Configure relationship with Post
            entity.HasMany(u => u.Posts)
                  .WithOne(p => p.User)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Post entity
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(p => p.Id);

            // Configure Title value object
            entity.OwnsOne(p => p.Title, title =>
            {
                title.Property(t => t.Value).HasColumnName("Title").IsRequired();
            });

            // Configure Content value object
            entity.OwnsOne(p => p.Content, content =>
            {
                content.Property(c => c.Value).HasColumnName("Content").IsRequired();
            });
        });
    }
}

