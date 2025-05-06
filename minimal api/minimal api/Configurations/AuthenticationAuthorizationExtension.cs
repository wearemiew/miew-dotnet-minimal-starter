using dotnet_starter.Domain.Entities;
using dotnet_starter.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace dotnet_starter.Configurations;
 
public static class AuthenticationAuthorizationExtension
{
    public static void AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        services.AddAuthorizationBuilder();
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();
    }
}