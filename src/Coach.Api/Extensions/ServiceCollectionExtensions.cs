using Coach.Application;
using Coach.Infrastructure;
using FluentValidation;

namespace Coach.Api.Extensions;

/// <summary>
/// Extension methods for configuring application services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();

        services.AddProblemDetails();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });

        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        services.AddInfrastructure(configuration);

        return services;
    }
}
