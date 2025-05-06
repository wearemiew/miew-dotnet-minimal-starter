namespace dotnet_starter.Features.Users;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users").WithTags("Users");

        // Get all users
        group.MapGet("/", async (UserService service) =>
        {
            var users = await service.GetAllUsersAsync();
            return Results.Ok(users);
        })
        .WithName("GetAllUsers")
        .WithOpenApi();

        // Get user by ID
        group.MapGet("/{id}", async (string id, UserService service) =>
        {
            try
            {
                var user = await service.GetUserByIdAsync(id);
                return Results.Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        })
        .WithName("GetUserById")
        .WithOpenApi();

        // Create user
        group.MapPost("/", async (CreateUserDto createUserDto, UserService service) =>
        {
            try
            {
                var user = await service.CreateUserAsync(createUserDto);
                return Results.Created($"/api/users/{user.Id}", user);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CreateUser")
        .WithOpenApi();

        // Update user
        group.MapPut("/{id}", async (string id, UpdateUserDto updateUserDto, UserService service) =>
        {
            try
            {
                var user = await service.UpdateUserAsync(id, updateUserDto);
                return Results.Ok(user);
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
        .WithName("UpdateUser")
        .WithOpenApi();

        // Delete user
        group.MapDelete("/{id}", async (string id, UserService service) =>
        {
            try
            {
                await service.DeleteUserAsync(id);
                return Results.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        })
        .WithName("DeleteUser")
        .WithOpenApi();

        return group;
    }
}