using Ardalis.Specification;

namespace Athlete.Domain.Repositories;

/// <summary>
/// Base repository interface for read operations.
/// Follows the Repository Pattern with Specification support.
/// </summary>
public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
{
}
