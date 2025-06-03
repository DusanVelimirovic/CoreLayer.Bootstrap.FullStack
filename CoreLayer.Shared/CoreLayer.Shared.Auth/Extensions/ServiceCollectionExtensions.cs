using CoreLayer.Shared.Auth.Interfaces;
using CoreLayer.Shared.Auth.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLayer.Shared.Auth;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreLayerAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}

