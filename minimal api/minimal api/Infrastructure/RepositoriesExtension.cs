using dotnet_starter.Application.Interfaces.Repositories;
using dotnet_starter.Infrastructure.Repositories;

namespace dotnet_starter.Infrastructure;

public static class RepositoriesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();

        return services;
    }
}