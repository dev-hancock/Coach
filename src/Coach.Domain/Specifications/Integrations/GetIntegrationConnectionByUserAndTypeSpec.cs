using Ardalis.Specification;
using Coach.Domain.Integrations;

namespace Coach.Domain.Specifications.Integrations;

/// <summary>
/// Specification to find an integration connection by user and type.
/// </summary>
public sealed class GetIntegrationConnectionByUserAndTypeSpec : Specification<Integration>, ISingleResultSpecification<Integration>
{
    public GetIntegrationConnectionByUserAndTypeSpec(Guid userId, IntegrationType type)
    {
        Query
            .Where(ic => ic.UserId == userId && ic.Type == type);
    }
}
