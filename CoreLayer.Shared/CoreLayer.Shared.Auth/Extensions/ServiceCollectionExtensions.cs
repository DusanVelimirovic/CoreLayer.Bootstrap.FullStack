using CoreLayer.Shared.Auth.Interfaces;
using CoreLayer.Shared.Auth.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLayer.Shared.Auth;

/// <summary>
/// Provides extension methods for registering CoreLayer authentication services.
/// </summary>
/// <remarks>
/// Use this class to inject authentication-related dependencies into the service collection.
/// </remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers CoreLayer authentication services into the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to which the services are added.</param>
    /// <param name="configuration">The application configuration (not used here but kept for future extensibility).</param>
    /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddCoreLayerAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}

