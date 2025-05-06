namespace dotnet_starter.Features.Users;

public record UserDto
{
    public required string Id { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record CreateUserDto
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public record UpdateUserDto
{
    public string? Username { get; init; }
    public string? Email { get; init; }
}