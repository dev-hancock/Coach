using Ardalis.Specification.EntityFrameworkCore;
using Coach.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Coach.Infrastructure.Persistence
{
    internal class Repository<T>(DbContext dbContext) : RepositoryBase<T>(dbContext), IRepository<T>
        where T : class;
}
