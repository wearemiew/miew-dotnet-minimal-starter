using dotnet_starter.Domain.Entities;

namespace dotnet_starter.Features.Posts.Mappers;

public static class PostMapper
{
    public static PostDto ToDto(Post post)
    {
        return new PostDto
        {
            Id = post.Id,
            Title = post.Title?.ToString() ?? string.Empty,
            Content = post.Content?.ToString() ?? string.Empty,
            UserId = post.UserId ?? string.Empty,
            UserName = post.User?.UserName,
            Status = post.Status,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt
        };
    }

    public static IEnumerable<PostDto> ToDtoList(IEnumerable<Post> posts)
    {
        return posts.Select(ToDto);
    }
}