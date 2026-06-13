using Coach.Application;
using Coach.Infrastructure;
using FluentValidation;

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

            // Add JWT Bearer authentication to Swagger
            options.AddSecurityDefinition("Bearer", new()
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
            });

            options.AddSecurityRequirement(new()
            {
                {
                    new()
                    {
                        Reference = new()
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddProblemDetails();

        // Register MediatR handlers from API layer
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

        // Add infrastructure layer (DbContext, Repositories, External Services, JWT Authentication)
        services.AddInfrastructure(configuration);

        return services;
    }
}
