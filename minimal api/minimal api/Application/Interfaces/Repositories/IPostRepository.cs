using dotnet_starter.Domain.Entities;
using dotnet_starter.Domain.Enums;

namespace dotnet_starter.Application.Interfaces.Repositories;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id);
    Task<IEnumerable<Post>> GetAllAsync();
    Task<IEnumerable<Post>> GetByUserIdAsync(string userId);
    Task<IEnumerable<Post>> GetByStatusAsync(PostStatus status);
    Task<Post> CreateAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(Guid id);
}