using dotnet_starter.Application.Interfaces.Repositories;
using dotnet_starter.Domain.Entities;
using dotnet_starter.Domain.ValueObjects;
using dotnet_starter.Features.Users.Mappers;

namespace dotnet_starter.Features.Users;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return UserMapper.ToDtoList(users);
    }

    public async Task<UserDto> GetUserByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        return UserMapper.ToDto(user);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // Check if user with email/username already exists
        if (await _userRepository.ExistsByEmailAsync(createUserDto.Email))
            throw new InvalidOperationException("Email is already in use");

        if (await _userRepository.ExistsByUsernameAsync(createUserDto.Username))
            throw new InvalidOperationException("Username is already taken");

        // Create a default Name or null based on available information
        Name? userName = null;
        // If we have any name information in the future, we can create a Name object here

        // Explicitly pass null as Name parameter which is now nullable
        var user = new User(
            createUserDto.Username,
            createUserDto.Email,
            userName // This is explicitly a Name? type which matches the constructor
        );

        await _userRepository.CreateAsync(user, createUserDto.Password);

        return UserMapper.ToDto(user);
    }

    public async Task<UserDto> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        // Check if email is already taken by another user
        if (updateUserDto.Email != null && updateUserDto.Email != user.Email)
        {
            var existingUser = await _userRepository.GetByEmailAsync(updateUserDto.Email);
            if (existingUser != null && existingUser.Id != id)
                throw new InvalidOperationException("Email is already in use");

            user.Email = updateUserDto.Email;
            user.NormalizedEmail = updateUserDto.Email.ToUpperInvariant();
        }

        // Check if username is already taken by another user
        if (updateUserDto.Username != null && updateUserDto.Username != user.UserName)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(updateUserDto.Username);
            if (existingUser != null && existingUser.Id != id)
                throw new InvalidOperationException("Username is already taken");

            user.UserName = updateUserDto.Username;
            user.NormalizedUserName = updateUserDto.Username.ToUpperInvariant();
        }

        await _userRepository.UpdateAsync(user);

        return UserMapper.ToDto(user);
    }

    public async Task DeleteUserAsync(string id)
    {
        await _userRepository.DeleteAsync(id);
    }
}