using Coach.Application;
using Coach.Infrastructure;
using Coach.Infrastructure.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Coach.Api.Extensions;

/// <summary>
/// Extension methods for configuring application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "Coach API",
                Version = "v1",
                Description = "API for managing triathlon training, coaching, and athlete performance tracking"
            });
        });

        services.AddProblemDetails();

        // Configure authentication with cookie-based identity
        services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();

        services.AddAuthorization();

        // Register SignInManager
        services.AddScoped<SignInManager<User>>();

        // Configure application cookie settings for API
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ExpireTimeSpan = TimeSpan.FromHours(24);
            options.SlidingExpiration = true;

            // Return 401 instead of redirecting for API endpoints
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
        });

        // Register MediatR handlers from API layer (for auth workflows)
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        });

        // Register FluentValidation validators from API layer
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }

    public static IServiceCollection AddApplicationLayers(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add application layer (MediatR, FluentValidation, ErrorOr)
        services.AddApplication();

        // Add infrastructure layer (DbContext, Repositories, External Services)
        services.AddInfrastructure(configuration);

        return services;
    }
}
