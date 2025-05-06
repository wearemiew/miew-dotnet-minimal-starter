using dotnet_starter.Domain.Entities;

namespace dotnet_starter.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<IEnumerable<User>> GetAllAsync();
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByUsernameAsync(string username);
    Task<User> CreateAsync(User user, string password);
    Task UpdateAsync(User user);
    Task DeleteAsync(string id);
}