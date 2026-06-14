using Coach.Application;
using Coach.Infrastructure;
using FluentValidation;
using Microsoft.OpenApi;

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
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
            });

            options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer"),
                    []
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
