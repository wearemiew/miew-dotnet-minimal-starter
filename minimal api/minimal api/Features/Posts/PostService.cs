using dotnet_starter.Application.Interfaces.Repositories;
using dotnet_starter.Domain.Entities;
using dotnet_starter.Domain.Enums;
using dotnet_starter.Domain.ValueObjects;
using dotnet_starter.Features.Posts.Mappers;

namespace dotnet_starter.Features.Posts;

public class PostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public PostService(IPostRepository postRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
    {
        var posts = await _postRepository.GetAllAsync();
        return PostMapper.ToDtoList(posts);
    }

    public async Task<PostDto> GetPostByIdAsync(Guid id)
    {
        var post = await _postRepository.GetByIdAsync(id);

        // Since the repository might return null (now that we've made it nullable),
        // we need to check for null before proceeding
        if (post == null)
            throw new KeyNotFoundException($"Post with ID {id} not found");

        return PostMapper.ToDto(post);
    }

    public async Task<IEnumerable<PostDto>> GetPostsByUserIdAsync(string userId)
    {
        var posts = await _postRepository.GetByUserIdAsync(userId);
        return PostMapper.ToDtoList(posts);
    }

    public async Task<IEnumerable<PostDto>> GetPostsByStatusAsync(PostStatus status)
    {
        var posts = await _postRepository.GetByStatusAsync(status);
        return PostMapper.ToDtoList(posts);
    }

    public async Task<PostDto> CreatePostAsync(string userId, CreatePostDto createPostDto)
    {
        // Validate that user exists
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found");

        // Create new post
        var post = new Post(
            Title.Create(createPostDto.Title),
            Content.Create(createPostDto.Content),
            user
        );

        await _postRepository.CreateAsync(post);

        return PostMapper.ToDto(post);
    }

    public async Task<PostDto> UpdatePostAsync(Guid id, UpdatePostDto updatePostDto)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
            throw new KeyNotFoundException($"Post with ID {id} not found");

        if (updatePostDto.Title != null || updatePostDto.Content != null)
        {
            var newTitle = updatePostDto.Title != null
                ? Title.Create(updatePostDto.Title)
                : post.Title;

            var newContent = updatePostDto.Content != null
                ? Content.Create(updatePostDto.Content)
                : post.Content;

            // Make sure neither value is null before updating
            if (newTitle != null && newContent != null)
            {
                post.Update(newTitle, newContent);
            }
        }

        if (updatePostDto.Status.HasValue)
        {
            if (updatePostDto.Status.Value == PostStatus.Published)
                post.Publish();
            else if (updatePostDto.Status.Value == PostStatus.Archived)
                post.Archive();
        }

        await _postRepository.UpdateAsync(post);

        return PostMapper.ToDto(post);
    }

    public async Task DeletePostAsync(Guid id)
    {
        await _postRepository.DeleteAsync(id);
    }
}