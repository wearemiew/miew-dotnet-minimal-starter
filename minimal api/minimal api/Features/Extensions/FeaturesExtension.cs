using dotnet_starter.Features.Posts;
using dotnet_starter.Features.Users;

namespace dotnet_starter.Features.Extensions;

public static class FeaturesExtension
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        // Register User feature services
        services.AddScoped<UserService>();

        // Register Post feature services
        services.AddScoped<PostService>();

        return services;
    }

    public static IEndpointRouteBuilder MapFeatureEndpoints(this IEndpointRouteBuilder app)
    {
        // Map all feature endpoints
        app.MapUserEndpoints();
        app.MapPostEndpoints();

        return app;
    }
}