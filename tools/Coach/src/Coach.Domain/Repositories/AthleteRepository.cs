using Coach.Domain.Athletes;

namespace Coach.Domain.Repositories;

/// <summary>
///     Repository interface for Athlete aggregate root.
///     Provides full CRUD access following the Repository Pattern with Specification support.
/// </summary>
public interface IAthleteRepository : IRepository<Athlete>
{
    // Add any custom Athlete-specific query methods here if needed
    // Example: Task<Athlete?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}