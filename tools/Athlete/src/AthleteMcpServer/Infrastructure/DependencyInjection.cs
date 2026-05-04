using Microsoft.Extensions.DependencyInjection;
using AthleteMcpServer.Domain.Repositories;
using AthleteMcpServer.Infrastructure.Repositories;

namespace AthleteMcpServer.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers all repository implementations with dependency injection.
    /// </summary>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        services.AddScoped<IAthleteRepository, AthleteRepository>();
        services.AddScoped<ITrainingGoalRepository, TrainingGoalRepository>();
        services.AddScoped<ITrainingPlanRepository, TrainingPlanRepository>();
        services.AddScoped<ICompletedRunRepository, CompletedRunRepository>();
        services.AddScoped<IFatigueEntryRepository, FatigueEntryRepository>();
        services.AddScoped<IInjuryEntryRepository, InjuryEntryRepository>();
        services.AddScoped<IRecoveryEntryRepository, RecoveryEntryRepository>();
        services.AddScoped<IShoeRepository, ShoeRepository>();
        services.AddScoped<ICoachDecisionRepository, CoachDecisionRepository>();

        return services;
    }
}
