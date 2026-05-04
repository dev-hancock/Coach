using AthleteMcpServer.Domain.Athletes;

namespace AthleteMcpServer.Domain.Repositories;

/// <summary>
/// Repository interface for Athlete aggregate root.
/// Provides read-only access following DDD principles.
/// </summary>
public interface IAthleteRepository : IReadRepository<Athlete>
{
    // Add any custom Athlete-specific query methods here if needed
    // Example: Task<Athlete?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

