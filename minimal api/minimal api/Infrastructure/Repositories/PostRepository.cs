using dotnet_starter.Application.Interfaces.Repositories;
using dotnet_starter.Domain.Entities;
using dotnet_starter.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace dotnet_starter.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _dbContext;

    public PostRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        var post = await _dbContext.Posts
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
            throw new KeyNotFoundException($"Post with ID {id} not found");

        return post;
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        return await _dbContext.Posts
            .Include(p => p.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetByUserIdAsync(string userId)
    {
        return await _dbContext.Posts
            .Where(p => p.UserId == userId)
            .Include(p => p.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetByStatusAsync(PostStatus status)
    {
        return await _dbContext.Posts
            .Where(p => p.Status == status)
            .Include(p => p.User)
            .ToListAsync();
    }

    public async Task<Post> CreateAsync(Post post)
    {
        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        _dbContext.Posts.Update(post);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await PostExistsAsync(post.Id))
                throw new KeyNotFoundException($"Post with ID {post.Id} not found");
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var post = await _dbContext.Posts.FindAsync(id);

        if (post == null)
            throw new KeyNotFoundException($"Post with ID {id} not found");

        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<bool> PostExistsAsync(Guid id)
    {
        return await _dbContext.Posts.AnyAsync(p => p.Id == id);
    }
}