using System.IdentityModel.Tokens.Jwt;
using System.Text;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Refit;
using Integration = Coach.Domain.Integrations.Integration;

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

        // Configure JWT settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Get JWT settings for authentication configuration
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
            ?? throw new InvalidOperationException("JwtSettings are not configured in appsettings.json");

        // Configure JWT authentication following the Code with Mukesh guide
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Keep the claim names exactly as they appear in the token (no surprise remapping)
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    // ClockSkew = TimeSpan.Zero makes the token expire exactly when it says it does
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = JwtRegisteredClaimNames.Name,
                    RoleClaimType = "role"
                };
            });

        services.AddAuthorization();

        // Register identity services
        services.AddScoped<IAuthService, Auth>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAthleteOnboardingService, AthleteOnboardingService>();

        // Register Strava integration with Refit
        services.Configure<StravaSettings>(configuration.GetSection(StravaSettings.SectionName));

        services.AddRefitClient<IStravaApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://www.strava.com"));

        // Register Strava as IIntegrationService
        services.AddScoped<IIntegrationService, StravaService>(sp =>
        {
            var api = sp.GetRequiredService<IStravaApi>();
            var repo = sp.GetRequiredService<IRepository<Integration>>();
            var onboarding = sp.GetRequiredService<IAthleteOnboardingService>();
            var settings = sp.GetRequiredService<IOptions<StravaSettings>>().Value;
            return new StravaService(api, repo, onboarding, settings);
        });

        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));

        return services;
    }
}
