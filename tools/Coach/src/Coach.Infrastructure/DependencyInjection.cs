using Coach.Application.Abstractions.Athletes;
using Coach.Application.Abstractions.Identity;
using Coach.Application.Abstractions.Integrations;
using Coach.Application.Abstractions.Integrations.Strava;
using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;
using Coach.Infrastructure.Identity;
using Coach.Infrastructure.Integrations.Strava;
using Coach.Infrastructure.Services;
using Coach.Infrastructure.Services.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddScoped<IIntegrationService, IntegrationService>();
        services.AddScoped<IAthleteOnboardingService, AthleteOnboardingService>();

        // Register Strava integration with Refit
        services.Configure<StravaSettings>(configuration.GetSection(StravaSettings.SectionName));

        services.AddRefitClient<IStravaApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://www.strava.com"));

        services.AddScoped<IStravaAuthService, StravaAuthService>();
        services.AddScoped<IStravaActivityService, StravaActivityService>();

        // Register generic repositories
        services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        // Register specific repositories
        services.AddScoped<IAthleteRepository, AthleteRepository>();
        services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
        services.AddScoped<ITrainingGoalRepository, TrainingGoalRepository>();
        services.AddScoped<ITrainingPlanRepository, TrainingPlanRepository>();
        services.AddScoped<ICompletedRunRepository, CompletedRunRepository>();
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<IInjuryEntryRepository, InjuryEntryRepository>();
        services.AddScoped<IFatigueEntryRepository, FatigueEntryRepository>();
        services.AddScoped<IRecoveryEntryRepository, RecoveryEntryRepository>();
        services.AddScoped<ICoachDecisionRepository, CoachDecisionRepository>();
        services.AddScoped<IBodyLocationRepository, BodyLocationRepository>();

        return services;
    }
}
