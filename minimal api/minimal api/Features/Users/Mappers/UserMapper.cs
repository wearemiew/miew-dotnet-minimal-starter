using dotnet_starter.Domain.Entities;

namespace dotnet_starter.Features.Users.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public static IEnumerable<UserDto> ToDtoList(IEnumerable<User> users)
    {
        return users.Select(ToDto);
    }
}