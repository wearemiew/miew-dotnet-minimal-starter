using dotnet_starter.Domain.Enums;

namespace dotnet_starter.Features.Posts;

public static class PostEndpoints
{
    public static RouteGroupBuilder MapPostEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/posts").WithTags("Posts");

        // Get all posts
        group.MapGet("/", async (PostService service) =>
        {
            var posts = await service.GetAllPostsAsync();
            return Results.Ok(posts);
        })
        .WithName("GetAllPosts")
        .WithOpenApi();

        // Get post by ID
        group.MapGet("/{id}", async (Guid id, PostService service) =>
        {
            try
            {
                var post = await service.GetPostByIdAsync(id);
                return Results.Ok(post);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        })
        .WithName("GetPostById")
        .WithOpenApi();

        // Get posts by user ID
        group.MapGet("/by-user/{userId}", async (string userId, PostService service) =>
        {
            var posts = await service.GetPostsByUserIdAsync(userId);
            return Results.Ok(posts);
        })
        .WithName("GetPostsByUserId")
        .WithOpenApi();

        // Get posts by status
        group.MapGet("/by-status/{status}", async (PostStatus status, PostService service) =>
        {
            var posts = await service.GetPostsByStatusAsync(status);
            return Results.Ok(posts);
        })
        .WithName("GetPostsByStatus")
        .WithOpenApi();

        // Create post
        group.MapPost("/{userId}", async (string userId, CreatePostDto createPostDto, PostService service) =>
        {
            try
            {
                var post = await service.CreatePostAsync(userId, createPostDto);
                return Results.Created($"/api/posts/{post.Id}", post);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CreatePost")
        .WithOpenApi();

        // Update post
        group.MapPut("/{id}", async (Guid id, UpdatePostDto updatePostDto, PostService service) =>
        {
            try
            {
                var post = await service.UpdatePostAsync(id, updatePostDto);
                return Results.Ok(post);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        })
        .WithName("UpdatePost")
        .WithOpenApi();

        // Delete post
        group.MapDelete("/{id}", async (Guid id, PostService service) =>
        {
            try
            {
                await service.DeletePostAsync(id);
                return Results.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        })
        .WithName("DeletePost")
        .WithOpenApi();

        return group;
    }
}