using Ardalis.Specification.EntityFrameworkCore;
using Coach.Application.Abstractions.Athletes;
using Coach.Application.Abstractions.Identity;
using Coach.Application.Abstractions.Integrations;
using Coach.Domain.Integrations;
using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;
using Coach.Infrastructure.Identity;
using Coach.Infrastructure.Integrations.Strava;
using Coach.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Coach.Infrastructure;

/// <summary>
/// Dependency injection registration for the infrastructure layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<AthleteDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Host=localhost;Database=coach;Username=postgres;Password=postgres";

            options.UseNpgsql(connectionString);
        });

        // Register Identity with Entity Framework stores
        services.AddIdentityCore<User>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                // User settings
                options.User.RequireUniqueEmail = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<AthleteDbContext>();

        // Register custom claims principal factory
        services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory>();

        // Register identity services
        services.AddScoped<IAuthService, Auth>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAthleteOnboardingService, AthleteOnboardingService>();

        // Register Strava integration with Refit
        services.Configure<StravaSettings>(configuration.GetSection(StravaSettings.SectionName));

        services.AddRefitClient<IStravaApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://www.strava.com"));

        // Register Strava as IIntegrationService
        services.AddScoped<IIntegrationService, StravaService>(sp =>
        {
            var api = sp.GetRequiredService<IStravaApi>();
            var repo = sp.GetRequiredService<IRepository<IntegrationConnection>>();
            var onboarding = sp.GetRequiredService<IAthleteOnboardingService>();
            var settings = sp.GetRequiredService<IOptions<StravaSettings>>().Value;
            return new StravaService(api, repo, onboarding, settings);
        });

        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));

        return services;
    }
}
