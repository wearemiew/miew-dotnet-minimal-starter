using dotnet_starter.Application.Interfaces.Repositories;
using dotnet_starter.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace dotnet_starter.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _dbContext;

    public UserRepository(UserManager<User> userManager, AppDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username) != null;
    }

    public async Task<User> CreateAsync(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create user: {errors}");
        }

        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to update user: {errors}");
        }
    }

    public async Task DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to delete user: {errors}");
        }
    }
}