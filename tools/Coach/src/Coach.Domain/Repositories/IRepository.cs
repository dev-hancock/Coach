using Ardalis.Specification;

namespace Coach.Domain.Repositories;

/// <summary>
///     Base repository interface for write operations.
///     Follows the Repository Pattern with Specification support.
///     Inherits from IReadRepository to provide full CRUD capabilities.
/// </summary>
public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}