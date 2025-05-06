using dotnet_starter.Domain.Enums;

namespace dotnet_starter.Features.Posts;

public record PostDto
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required string UserId { get; init; }
    public string? UserName { get; init; }
    public PostStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record CreatePostDto
{
    public required string Title { get; init; }
    public required string Content { get; init; }
    public PostStatus Status { get; init; } = PostStatus.Draft;
}

public record UpdatePostDto
{
    public string? Title { get; init; }
    public string? Content { get; init; }
    public PostStatus? Status { get; init; }
}